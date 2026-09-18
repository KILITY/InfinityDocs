using System.IO;
using InfinityDocs.Features.Helpers.Implementations;
using InfinityDocs.Features.Helpers.Interfaces;
using InfinityDocs.Features.Models;
using InfinityDocs.Features.Models.FileStructure;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Parser;

public class DocParser : IDocParser
{
    private IFileExtensionHelper _fileExtensionHelper { get; set; }  //ToDo : Inject this dependency
    private ILanguageHelper _languageHelper { get; set; } //ToDo : Inject this dependency

    public DocParser(IFileExtensionHelper fileExtensionHelper, ILanguageHelper languageHelper)
    {
        _fileExtensionHelper = fileExtensionHelper;
        _languageHelper = languageHelper;
    }

    public List<DocFile> Parse(string folderPath, Languages language)
    {
        //1. Open Folder
        var filePaths = GetFilePaths(folderPath, language);

        //2. Get all files for the language
        var fileParseResults = ParseFiles(filePaths, language);

        //Parse Each File and Create File Object with Methods, MethodInvocations, Classes, ClassInvocations
        foreach (var fileParseResult in fileParseResults)
        {
            var fileName = "";
            var filePath = "";
            var fileExtension = "";

            var file = new DocFile(fileName, filePath, fileExtension);


        }

        return new List<DocFile>();
    }

    private List<string> GetFilePaths(string folderPath, Languages language)
    {
        var fileExtension = _fileExtensionHelper.GetFileExtension(language);

        var filePaths = Directory.GetFiles(folderPath, $"*{fileExtension}", SearchOption.AllDirectories).ToList();
        return filePaths;
    }

    private List<DocFile> ParseFiles(List<string> filePaths, Languages language)
    {
        var results = new List<DocFile>();
        var languageString = _languageHelper.GetLanguageName(language);
        var parser = TreeSitterLanguagePackConverter.GetParser(languageString);
        var MethodInvocationDictionary = new Dictionary<Method, List<MethodInvocation>>();
        var ClassInvocationsDictioanry = new Dictionary<Class, List<ClassInvocation>>();

        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            var fileExtension = Path.GetExtension(filePath);
            var file = new DocFile(fileName, filePath, fileExtension);

            var fileText = File.ReadAllText(filePath);
            var root = parser.Parse(fileText)?.RootNode();

            if (root is not null)
            {
                TraverseTree(root, file, language);
            }
        }

        return results;
    }

    private void TraverseTree(Node node, DocFile file, Languages language)
    {
        var config = _languageHelper.GetLanguageConfiguration(language);

        if (config.MethodDeclarationNodeTypes.Contains(node.Kind()))
        {
            HandleMethodDeclaration(node);
        }

        if(config.MethodInvocationNodeTypes.Contains(node.Kind()))
        {
            HandleMethodInvocation(node);
        }

        if (config.ClassDeclarationNodeTypes.Contains(node.Kind()))
        {
            HandleClassDeclaration(node);
        }

        if (config.ClassInvocationNodeTypes.Contains(node.Kind()))
        {
            HandleClassInvocation(node);
        }
    }

    private void HandleClassInvocation(Node node)
    {
        throw new NotImplementedException();
    }

    private void HandleClassDeclaration(Node node)
    {
        throw new NotImplementedException();
    }

    private void HandleMethodInvocation(Node node)
    {
        throw new NotImplementedException();
    }

    private void HandleMethodDeclaration(Node node)
    {
        throw new NotImplementedException();
    }
}