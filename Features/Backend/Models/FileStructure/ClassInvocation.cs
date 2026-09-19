using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityDocs.Features.Models.FileStructure
{
    public class ClassInvocation
    {
        public string ClassName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public ClassInvocation(string className, string filePath, string fileName)
        {
            this.ClassName = className;
            this.FilePath = filePath;
            this.FileName = fileName;
        }
    }
}
