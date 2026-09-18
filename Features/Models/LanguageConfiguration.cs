using TreeSitter;

namespace InfinityDocs.Features.Models
{
    public class LanguageConfiguration
    {
        public Language Language { get; set; }
        public HashSet<string> MethodDeclarationNodeTypes { get; set; }
        public HashSet<string> MethodInvocationNodeTypes { get; set; }
        public HashSet<string> ClassDeclarationNodeTypes { get; set; }
        public HashSet<string> ClassInvocationNodeTypes { get; set; }

        public LanguageConfiguration(Language language)
        {
            Language = language;
            MethodDeclarationNodeTypes = new HashSet<string>();
            MethodInvocationNodeTypes = new HashSet<string>();
            ClassDeclarationNodeTypes = new HashSet<string>();
            ClassInvocationNodeTypes = new HashSet<string>();
        }
    }
}
