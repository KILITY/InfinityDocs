namespace InfinityDocs.Features.Models.FileStructure
{
    public class Method
    {
        public string Name { get; set; } = string.Empty;
        public string AccessModifier { get; set; } = string.Empty;
        public Class ReturnType { get; set; }
        public Class[] Parameters { get; set; } = [];
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public MethodInvocation[] Invocations { get; set; } = [];

        public Method(string Name, string AccessModifier, Class ReturnType, Class[] Parameters, string FilePath, string FileName)
        {
            this.Name = Name;
            this.AccessModifier = AccessModifier;
            this.ReturnType = ReturnType;
            this.Parameters = Parameters;
            this.FilePath = FilePath;
            this.FileName = FileName;
        }
    }
}
