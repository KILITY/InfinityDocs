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

        public ClassInvocation(string ClassName, string FilePath, string FileName)
        {
            this.ClassName = ClassName;
            this.FilePath = FilePath;
            this.FileName = FileName;
        }
    }
}
