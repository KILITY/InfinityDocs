namespace InfinityDocs.Features.Models.FileStructure
{
    public class Parameter
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public Parameter(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
