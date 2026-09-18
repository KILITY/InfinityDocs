using System;
using System.Collections.Generic;
using System.Text;
using InfinityDocs.Features.Extractors.Interface;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Extractors.Implementations
{
    public class CSharpExtractor : INodeExtractor
    {
        public Node? GetDeclarationName(Node node) => node.ChildByFieldName("name");
        public Node? GetMethodReturnType(Node node) => node.ChildByFieldName("returns");
        public Node? GetMethodParameters(Node node) => node.ChildByFieldName("parameters");
        public Node? GetMethodInvocationFunction(Node node) => node.ChildByFieldName("function");
        public Node? GetClassInvocationType(Node node) => node.ChildByFieldName("type");
    }
}
