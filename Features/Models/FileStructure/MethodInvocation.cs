using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityDocs.Features.Models.FileStructure
{
    public class MethodInvocation
    {
        public string InvokedMethodName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public MethodInvocation(string InvokedMethodName, string MethodName, string FilePath, string FileName)
        {
            this.InvokedMethodName = InvokedMethodName;
            this.MethodName = MethodName;
            this.FilePath = FilePath;
            this.FileName = FileName;
        }
    }
}
