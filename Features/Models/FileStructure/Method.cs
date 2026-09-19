namespace InfinityDocs.Features.Models.FileStructure
{
    public class Method
    {
        public string Name { get; set; } = string.Empty;
        public string? AccessModifier { get; set; } = string.Empty;
        public string ReturnType { get; set; } = string.Empty;
        public List<Parameter> Parameters { get; set; } = [];
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public List<MethodInvocation> Invocations { get; set; } = [];

        public Method(string name, string returnType, List<Parameter> parameters, string filePath, string fileName, string? accessModifier = null)
        {
            this.Name = name;
            this.AccessModifier = accessModifier;
            this.ReturnType = returnType;
            this.Parameters = parameters;
            this.FilePath = filePath;
            this.FileName = fileName;
        }
    }
}
