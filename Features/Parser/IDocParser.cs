using InfinityDocs.Features.Models;
using InfinityDocs.Features.Models.FileStructure;

namespace InfinityDocs.Features.Parser;

public interface IDocParser
{
    List<DocFile> Parse(string folderPath, Languages language);
}