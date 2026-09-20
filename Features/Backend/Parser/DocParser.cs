using System.IO;
using System.Text;
using InfinityDocs.Features.Extractors.Implementations;
using InfinityDocs.Features.Extractors.Interface;
using InfinityDocs.Features.Helpers.Interfaces;
using InfinityDocs.Features.Models;
using InfinityDocs.Features.Models.FileStructure;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Parser;

public class DocParser : IDocParser
{
    private IFileExtensionHelper _fileExtensionHelper { get; set; }  //ToDo : Inject this dependency
    private ILanguageHelper _languageHelper { get; set; } //ToDo : Inject this dependency
    private INodeHelper _nodeHelper { get; set; } //ToDo : Inject this dependency
    private INodeExtractor _extractor { get; set; } //ToDo : Inject this dependency
    private Languages _language { get; }

    public DocParser(Languages language, IFileExtensionHelper fileExtensionHelper, ILanguageHelper languageHelper, INodeHelper nodeHelper, INodeExtractor extractor)
    {
        _language = language;
        _fileExtensionHelper = fileExtensionHelper;
        _languageHelper = languageHelper;
        _nodeHelper = nodeHelper;
        _extractor = extractor;
    }

    public List<DocFile> Parse(string folderPath)
    {

        var filePaths = GetFilePaths(folderPath);
        var fileParseResults = ParseFiles(filePaths);

        LinkFiles(fileParseResults);

        return fileParseResults;
    }



    #region Helper Methods

    private List<Parameter> CreateParameterObjects(List<Node>? parameterNodes, byte[] fileBytes)
    {
        var parameters = new List<Parameter>();
        if (parameterNodes is not null)
        {
            foreach (var parameterNode in parameterNodes)
            {
                var nameNode = _extractor.GetParameterName(parameterNode);
                var typeNode = _extractor.GetParameterType(parameterNode);

                if (nameNode is null || typeNode is null)
                {
                    continue;
                }

                var type = _nodeHelper.GetNodeText(typeNode, fileBytes);
                var name = _nodeHelper.GetNodeText(nameNode, fileBytes);

                var parameter = new Parameter(type, name);
                parameters.Add(parameter);
            }
        }

        return parameters;
    }

    #endregion

    #region File Parsing Methods
    private List<string> GetFilePaths(string folderPath)
    {
        var fileExtension = _fileExtensionHelper.GetFileExtension(_language);

        var filePaths = Directory.GetFiles(folderPath, $"*{fileExtension}", SearchOption.AllDirectories).ToList();
        return filePaths;
    }

    private List<DocFile> ParseFiles(List<string> filePaths)
    {
        var results = new List<DocFile>();
        var languageString = _languageHelper.GetLanguageName(_language);
        var parser = TreeSitterLanguagePackConverter.GetParser(languageString);

        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            var fileExtension = Path.GetExtension(filePath);
            var file = new DocFile(filePath, fileName, fileExtension);

            var fileText = File.ReadAllText(filePath);
            var fileBytes = Encoding.UTF8.GetBytes(fileText);
            var root = parser.Parse(fileText)?.RootNode();

            if (root is not null)
            {
                TraverseTree(root, file, fileBytes);
            }
            results.Add(file);
        }

        return results;
    }

    private void TraverseTree(Node node, DocFile file, Byte[] fileBytes)
    {
        var config = _languageHelper.GetLanguageConfiguration(_language);
        var kind = node.Kind();
        if (config.MethodDeclarationNodeKinds.Contains(kind))
        {
            HandleMethodDeclaration(node, file, fileBytes);
        }

        if (config.MethodInvocationNodeKinds.Contains(kind))
        {
            HandleMethodInvocation(node, file, fileBytes);
        }

        if (config.ClassDeclarationNodeKinds.Contains(kind))
        {
            HandleClassDeclaration(node, file, fileBytes);
        }

        if (config.ClassInvocationNodeKinds.Contains(kind))
        {
            HandleClassInvocation(node, file, fileBytes);
        }

        for (uint i = 0; i < node.ChildCount(); i++)
        {
            TraverseTree(node.Child(i), file, fileBytes);
        }
    }

    private void HandleClassInvocation(Node node, DocFile file, Byte[] fileBytes)
    {
        var typeNode = _extractor.GetClassInvocationType(node);
        var classInvocation = new ClassInvocation(_nodeHelper.GetNodeText(typeNode, fileBytes), file.FilePath, file.FileName);
        file.ClassInvocations.Add(classInvocation);
    }

    private void HandleClassDeclaration(Node node, DocFile file, Byte[] fileBytes)
    {
        var classNode = _extractor.GetDeclarationName(node);
        var classObject = new Class(_nodeHelper.GetNodeText(classNode, fileBytes), file.FilePath, file.FileName);

        if (_extractor.SupportAccessModifier(_language))
        {
            var accessModifier = _extractor.GetAccessModifier(node, fileBytes);
            if (accessModifier is not null)
            {
                classObject.AccessModifier = _nodeHelper.GetNodeText(accessModifier, fileBytes);
            }
        }

        file.Classes.Add(classObject);
    }

    private void HandleMethodInvocation(Node node, DocFile file, Byte[] fileBytes)
    {
        var methodInvocationNode = _extractor.GetMethodInvocationFunction(node);
        var methodInvocation = new MethodInvocation(_nodeHelper.GetNodeText(methodInvocationNode, fileBytes), file.FilePath, file.FileName);
        file.MethodInvocations.Add(methodInvocation);
    }

    private void HandleMethodDeclaration(Node node, DocFile file, Byte[] fileBytes)
    {
        var methodNode = _extractor.GetDeclarationName(node);

        var returnTypeNode = _extractor.GetMethodReturnType(node);
        var returnType = _nodeHelper.GetNodeText(returnTypeNode, fileBytes);

        var parameterNodes = _extractor.GetMethodParameterNodes(node);
        var parameters = CreateParameterObjects(parameterNodes, fileBytes);

        var methodObject = new Method(_nodeHelper.GetNodeText(methodNode, fileBytes), returnType, parameters, file.FilePath, file.FileName);

        if (_extractor.SupportAccessModifier(_language))
        {
            var accessModifier = _extractor.GetAccessModifier(node, fileBytes);
            if (accessModifier is not null)
            {
                methodObject.AccessModifier = _nodeHelper.GetNodeText(accessModifier, fileBytes);
            }
        }

        file.Methods.Add(methodObject);
    }

    #endregion

    #region File Linking Methods

    private void LinkFiles(List<DocFile> fileParseResults)
    {
        var methodDictionary = CreateMethodInvocationDictionary(fileParseResults); //for each method all it's invocations
        var classDictionary = CreateClassInvocationDictionary(fileParseResults); //for each class all it's invocations

        //file.LinkedFileNames --> means the files that invoke this file in some sort of way

        foreach (var file in fileParseResults)
        {

            foreach (var method in file.Methods)
            {
                if(methodDictionary.TryGetValue(method, out var methodInvocations))
                {
                    file.DependentFiles.UnionWith(methodInvocations.Select(mi => mi.FileName));
                }
            }

            foreach (var @class in file.Classes)
            {
                if (classDictionary.TryGetValue(@class, out var classInvocations))
                {
                    file.DependentFiles.UnionWith(classInvocations.Select(ci => ci.FileName));
                }
            }
        }
    }

    private Dictionary<Method, List<MethodInvocation>> CreateMethodInvocationDictionary(List<DocFile> docFiles)
    {
        var allMethods = docFiles.SelectMany(f => f.Methods).ToList();
        var allMethodInvocations = docFiles.SelectMany(f => f.MethodInvocations).ToList();

        var dictionary = new Dictionary<Method, List<MethodInvocation>>();

        foreach (var method in allMethods)
        {
            dictionary[method] = allMethodInvocations.Where(mi => mi.Method == method.Name).ToList();
        }
        return dictionary;
    }

    private Dictionary<Class, List<ClassInvocation>> CreateClassInvocationDictionary(List<DocFile> docFiles)
    {
        var allClasses = docFiles.SelectMany(f => f.Classes).ToList();
        var allClassInvocations = docFiles.SelectMany(f => f.ClassInvocations).ToList();

        var dictionary = new Dictionary<Class, List<ClassInvocation>>();

        foreach (var @class in allClasses)
        {
            dictionary[@class] = allClassInvocations.Where(ci => ci.ClassName == @class.Name).ToList();
        }
        return dictionary;
    }
    #endregion
}