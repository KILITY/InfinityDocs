using System.Text;
using InfinityDocs.Features.Helpers.Interfaces;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Helpers.Implementations
{
    public class NodeHelper : INodeHelper
    {
        public string GetNodeText(Node node, Byte[] fileBytes)
        {
            var start = (int)node.StartByte();
            var length = (int)(node.EndByte() - node.StartByte());

            return Encoding.UTF8.GetString(fileBytes, start, length);
        }
    }
}
