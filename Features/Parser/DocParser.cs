using System.IO;
using System.Text;
using InfinityDocs.Features.Extractors.Implementations;
using InfinityDocs.Features.Extractors.Interface;
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

        var filePaths = GetFilePaths(folderPath, language);
        var extractor = ExtractorFactory.CreateExtractor(language);
        var fileParseResults = ParseFiles(filePaths, extractor, language);

        return new List<DocFile>();
    }

    private List<string> GetFilePaths(string folderPath, Languages language)
    {
        var fileExtension = _fileExtensionHelper.GetFileExtension(language);

        var filePaths = Directory.GetFiles(folderPath, $"*{fileExtension}", SearchOption.AllDirectories).ToList();
        return filePaths;
    }

    private List<DocFile> ParseFiles(List<string> filePaths, INodeExtractor extractor, Languages language)
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
            var file = new DocFile(filePath, fileName, fileExtension);

            var fileText = File.ReadAllText(filePath);
            var fileBytes = Encoding.UTF8.GetBytes(fileText);
            var root = parser.Parse(fileText)?.RootNode();

            if (root is not null)
            {
                TraverseTree(root, file, fileBytes, extractor, language);
            }
        }

        return results;
    }

    private void TraverseTree(Node node, DocFile file, Byte[] fileBytes, INodeExtractor extractor, Languages language)
    {
        var config = _languageHelper.GetLanguageConfiguration(language);
        var kind = node.Kind();
        //if (config.MethodDeclarationNodeKinds.Contains(node.Kind()))
        //{
        //    HandleMethodDeclaration(node, file);
        //}

        //if(config.MethodInvocationNodeKinds.Contains(node.Kind()))
        //{
        //    HandleMethodInvocation(node, file);
        //}

        //if (config.ClassDeclarationNodeKinds.Contains(node.Kind()))
        //{
        //    HandleClassDeclaration(node, file);
        //}

        if (config.ClassInvocationNodeKinds.Contains(kind))
        {
            HandleClassInvocation(node, file, fileBytes, extractor);
        }

        for (uint i = 0; i < node.ChildCount(); i++)
        {
            TraverseTree(node.Child(i), file, fileBytes, extractor, language);
        }
    }

    private void HandleClassInvocation(Node node, DocFile file, Byte[] fileBytes, INodeExtractor extractor)
    {
        var typeNode = extractor.GetClassInvocationType(node);
        file.ClassInvocations.Add(new ClassInvocation(GetNodeText(typeNode, fileBytes), file.FilePath, file.FileName));
    }

    private void HandleClassDeclaration(Node node, DocFile file, INodeExtractor extractor)
    {
        throw new NotImplementedException();
    }

    private void HandleMethodInvocation(Node node, DocFile file, INodeExtractor extractor)
    {
        throw new NotImplementedException();
    }

    private void HandleMethodDeclaration(Node node, DocFile file, INodeExtractor extractor)
    {
        throw new NotImplementedException();
    }

    private string GetNodeText(Node node, Byte[] fileBytes)
    {
        var start = (int)node.StartByte();
        var length = (int)(node.EndByte() - node.StartByte());

        return Encoding.UTF8.GetString(fileBytes, start, length);
    }
}