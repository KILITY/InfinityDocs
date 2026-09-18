using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Extractors.Interface
{
    public interface INodeExtractor
    {
        Node? GetDeclarationName(Node node);
        Node? GetMethodReturnType(Node node);
        Node? GetMethodParameters(Node node);
        Node? GetMethodInvocationFunction(Node node);
        Node? GetClassInvocationType(Node node);
    }
}
