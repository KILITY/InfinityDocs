
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Helpers.Interfaces
{
    public interface INodeHelper
    {
        string GetNodeText(Node node, byte[] fileBytes);
    }
}
