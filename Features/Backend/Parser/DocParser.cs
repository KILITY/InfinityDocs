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

        //var linkedFiles = LinkFiles(fileParseResults);

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

    //private List<DocFile> LinkFiles(List<DocFile> fileParseResults)
    //{
    //    var methodDictionary = CreateMethodInvocationDictionary(fileParseResults);
    //    var classDictionary = CreateClassInvocationDictionary(fileParseResults);

    //    //Now we know for each method and class what invocations are in the project, we can link them to the methods and classes
    //    //What we actually want is not to link the invocations to the methods and classes, but to build a graph out of files. For example
    //    //There is a link between two files, if a method or class declared in file A is invoked in file B. This project is meant to showcase the relationship between files, not between methods and classes.
    //    //This will provide a better understanding of the project structure and how files are related to each other, of coupling, and can give a better understanding of how Ai changes impact projects.
    //    //

    //}

    private Dictionary<Method, MethodInvocation> CreateClassInvocationDictionary(List<DocFile> docFiles)
    {
        var dictionary = new Dictionary<Method, MethodInvocation>();

        var methods = docFiles.SelectMany(df => df.Methods);
        var methodInvocations = docFiles.SelectMany(df => df.MethodInvocations);

        foreach (var method in methods)
        {
            foreach(var methodInvocation in methodInvocations)
            {
                if (methodInvocation.Method == method.Name)
                {
                    dictionary.Add(method, methodInvocation);
                }
            }
        }

        return dictionary;
    }

    private Dictionary<Class, ClassInvocation> CreateMethodInvocationDictionary(List<DocFile> docFiles)
    {
        var dictionary = new Dictionary<Class, ClassInvocation>();

        var classes = docFiles.SelectMany(df => df.Classes);
        var classInvocations = docFiles.SelectMany(df => df.ClassInvocations);

        foreach (var @class in classes)
        {
            foreach (var classInvocation in classInvocations)
            {
                if (classInvocation.ClassName == @class.Name)
                {
                    dictionary.Add(@class, classInvocation);
                }
            }
        }

        return dictionary;
    }
    #endregion
}