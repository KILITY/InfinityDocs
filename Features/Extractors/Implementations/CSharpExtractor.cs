using InfinityDocs.Features.Extractors.Interface;
using InfinityDocs.Features.Helpers.Implementations;
using InfinityDocs.Features.Helpers.Interfaces;
using InfinityDocs.Features.Models;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Extractors.Implementations
{
    public class CSharpExtractor : INodeExtractor
    {
        private INodeHelper _nodeHelper = new NodeHelper(); //ToDo : Inject this dependency instead of creating a new instance

        public Node? GetDeclarationName(Node node) => node.ChildByFieldName("name");
        public Node? GetMethodReturnType(Node node) => node.ChildByFieldName("returns");

        public List<Node>? GetMethodParameterNodes(Node node)
        {
            var parametersNode = node.ChildByFieldName("parameters");
            var result = new List<Node?>();

            if (parametersNode is not null)
            {
                for (uint i = 0; i < parametersNode.ChildCount(); i++)
                {
                    var parameterNode = parametersNode.Child(i);

                    if (parameterNode is not null)
                    {
                        result.Add(parameterNode);
                    }
                }
            }
            return result;
        } 
        public Node? GetMethodInvocationFunction(Node node) => node.ChildByFieldName("function");
        public Node? GetClassInvocationType(Node node) => node.ChildByFieldName("type");
        public Node? GetParameterType(Node node) => node.ChildByFieldName("type");
        public Node? GetParameterName(Node node) => node.ChildByFieldName("name");

        public bool SupportAccessModifier(Languages language) => true;

        public Node? GetAccessModifier(Node node, byte[] fileBytes)
        {
            for (uint i = 0; i < node.ChildCount(); i++)
            {
                var child = node.Child(i);

                if(child == null)
                {
                    continue;
                }

                var text = _nodeHelper.GetNodeText(child, fileBytes);

                if(text is "public" or "private" or "protected")
                {
                    return child;
                }
            }
            return null;
        }
    }
}
