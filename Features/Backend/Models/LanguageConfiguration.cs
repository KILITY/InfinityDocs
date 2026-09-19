using TreeSitter;

namespace InfinityDocs.Features.Models
{
    public class LanguageConfiguration
    {
        public Language Language { get; set; }
        public HashSet<string> MethodDeclarationNodeKinds { get; set; }
        public HashSet<string> MethodInvocationNodeKinds { get; set; }
        public HashSet<string> ClassDeclarationNodeKinds { get; set; }
        public HashSet<string> ClassInvocationNodeKinds { get; set; }

        public LanguageConfiguration(Language language)
        {
            Language = language;
            MethodDeclarationNodeKinds = new HashSet<string>();
            MethodInvocationNodeKinds = new HashSet<string>();
            ClassDeclarationNodeKinds = new HashSet<string>();
            ClassInvocationNodeKinds = new HashSet<string>();
        }
    }
}
