using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityDocs.Features.Models.FileStructure
{
    public class MethodInvocation
    {
        public string Method { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public MethodInvocation(string method, string filePath, string fileName)
        {
            this.Method = method;
            this.FilePath = filePath;
            this.FileName = fileName;
        }
    }
}
