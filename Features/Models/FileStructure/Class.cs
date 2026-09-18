namespace InfinityDocs.Features.Models.FileStructure
{
    public class Class
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string AccessModifier { get; set; } = string.Empty;

        public Class(string Name, string FilePath, string FileName, string AccessModifier)
        {
            this.Name = Name;
            this.FilePath = FilePath;
            this.FileName = FileName;
            this.AccessModifier = AccessModifier;
        }
    }
}
