namespace InfinityDocs.Features.Models.FileStructure
{
    public class DocFile
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public Method[] Methods { get; set; } = [];
        public MethodInvocation[] MethodInvocations { get; set; } = [];
        public Class[] Classes { get; set; } = [];
        public ClassInvocation[] ClassInvocations { get; set; } = [];

        public DocFile(string filePath, string fileName, string fileExtension)
        {
            FilePath = filePath;
            FileName = fileName;
            FileExtension = fileExtension;
        }
    }
}
