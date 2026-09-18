using System;
using System.Collections.Generic;
using System.Text;
using InfinityDocs.Features.Models;

namespace InfinityDocs.Features.Helpers.Interfaces
{
    public interface ILanguageHelper
    {
        public string GetLanguageName(Languages language);

        public LanguageConfiguration GetLanguageConfiguration(Languages language);
    }
}
