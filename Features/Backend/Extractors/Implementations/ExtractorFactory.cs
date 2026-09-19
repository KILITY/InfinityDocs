using InfinityDocs.Features.Extractors.Interface;
using InfinityDocs.Features.Models;

namespace InfinityDocs.Features.Extractors.Implementations
{
    public class ExtractorFactory
    {
        public static INodeExtractor CreateExtractor(Languages language)
        {
            return language switch{
                Languages.csharp => new CSharpExtractor(),
                _ => throw new ArgumentException("Unsupported language")
            };
        }

    }
}
