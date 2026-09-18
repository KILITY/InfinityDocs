using System;
using System.Collections.Generic;
using System.Text;
using InfinityDocs.Features.Helpers.Interfaces;
using InfinityDocs.Features.Models;
using TreeSitterLanguagePack;

namespace InfinityDocs.Features.Helpers.Implementations
{
    public class LanguageHelper : ILanguageHelper
    {
        public string GetLanguageName(Languages language)
        {
            return language switch
            {
                Languages.csharp => "csharp",
                _ => throw new ArgumentOutOfRangeException(nameof(language))
            };
        }

        public LanguageConfiguration GetLanguageConfiguration(Languages language)
        {

            var treeSitterlanguage = TreeSitterLanguagePackConverter.GetLanguage(language.ToString());
            var languageConfiguration = new LanguageConfiguration(treeSitterlanguage);
            switch (language)
            {
                case Languages.csharp:
                    languageConfiguration.MethodDeclarationNodeTypes.Add("method_declaration");
                    languageConfiguration.MethodInvocationNodeTypes.Add("invocation_expression");
                    languageConfiguration.ClassDeclarationNodeTypes.Add("class_declaration");
                    languageConfiguration.ClassInvocationNodeTypes.Add("object_creation_expression");
                    break;
                default:
                    throw new NotImplementedException(nameof(language));
            }

            return languageConfiguration;
        }
    }
}
