namespace InfinityDocs.Features.Models.FileStructure
{
    public class Class
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? AccessModifier { get; set; } = null;

        public Class(string name, string filePath, string fileName, string? accessModifier = null)
        {
            this.Name = name;
            this.FilePath = filePath;
            this.FileName = fileName;
            this.AccessModifier = accessModifier;
        }
    }
}
