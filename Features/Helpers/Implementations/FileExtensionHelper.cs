using System;
using System.Collections.Generic;
using System.Text;
using InfinityDocs.Features.Helpers.Interfaces;
using InfinityDocs.Features.Models;

namespace InfinityDocs.Features.Helpers.Implementations
{
    public class FileExtensionHelper : IFileExtensionHelper
    {

        public string GetFileExtension(Languages language)
        {
            return language switch
            {
                Languages.csharp => ".cs",
                _ => throw new NotImplementedException($"File extension for {language} is not implemented.")
            };
        }
    }
}
