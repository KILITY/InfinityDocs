using InfinityDocs.Features.Models;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Extractors.Interface
{
    public interface INodeExtractor
    {
        Node? GetDeclarationName(Node node);
        Node? GetMethodReturnType(Node node);
        Node? GetParameterName(Node node);
        Node? GetParameterType(Node node);
        List<Node>? GetMethodParameterNodes(Node node);
        Node? GetMethodInvocationFunction(Node node);
        Node? GetClassInvocationType(Node node);
        bool SupportAccessModifier(Languages language);
        Node? GetAccessModifier(Node node, byte[] fileBytes);
    }
}
