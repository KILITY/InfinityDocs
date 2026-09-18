namespace InfinityDocs.Features.Models.FileStructure
{
    public class Property
    {
        public string Name { get; set; } = string.Empty;
        public string AccessModifier { get; set; } = string.Empty;
        public Class Type { get; set; }
        public string FilePath { get; set; } = string.Empty;

        public Property(string Name, string AccessModifier, Class Type, string FilePath)
        {
            this.Name = Name;
            this.AccessModifier = AccessModifier;
            this.Type = Type;
            this.FilePath = FilePath;
        }
    }
}
