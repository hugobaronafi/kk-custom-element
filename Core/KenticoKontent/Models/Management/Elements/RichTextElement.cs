using System.Collections.Generic;

using Core.KenticoKontent.Models.Management.Items;

namespace Core.KenticoKontent.Models.Management.Elements
{
    public class RichTextElement : AbstractElement<string>
    {
        public const string Type = "rich_text";

        public IEnumerable<Component>? Components { get; set; }
    }
}