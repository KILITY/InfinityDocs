namespace InfinityDocs.Features.Models.FileStructure
{
    public class DocFile
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public List<Method> Methods { get; set; } = [];
        public List<MethodInvocation> MethodInvocations { get; set; } = [];
        public List<Class> Classes { get; set; } = [];
        public List<ClassInvocation> ClassInvocations { get; set; } = [];

        public List<string> LinkedFileNames { get; set; } = [];

        public DocFile(string filePath, string fileName, string fileExtension)
        {
            FilePath = filePath;
            FileName = fileName;
            FileExtension = fileExtension;
        }
    }
}
