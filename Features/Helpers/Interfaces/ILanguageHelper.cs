using System;
using System.Collections.Generic;
using System.Text;
using InfinityDocs.Features.Models;

namespace InfinityDocs.Features.Helpers.Interfaces
{
    public interface ILanguageHelper
    {
        string GetLanguageName(Languages language);

        LanguageConfiguration GetLanguageConfiguration(Languages language);
    }
}
