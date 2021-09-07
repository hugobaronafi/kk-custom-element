using System.Collections.Generic;

namespace Core.AzureTranslator.Services
{
    public interface ITextAnalyzer
    {
        IEnumerable<string> SplitHtml(string input);
    }
}