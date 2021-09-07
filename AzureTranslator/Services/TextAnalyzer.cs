using System.Collections.Generic;
using System.Linq;

using Core.AzureTranslator.Services;

using HtmlAgilityPack;

namespace AzureTranslator.Services
{
    public class TextAnalyzer : ITextAnalyzer
    {
        public IEnumerable<string> SplitHtml(string input)
        {
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(input);

            return htmlDocument.DocumentNode.ChildNodes.Select(node => node.OuterHtml);
        }
    }
}