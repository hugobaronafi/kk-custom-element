namespace Core.KenticoKontent.Models.Management.Elements
{
    public class LinkedItemsElement : AbstractReferenceListElement
    {
        public const string Type = "modular_content";

        internal LinkedItemsElement(AbstractReferenceListElement element)
        {
            Element = element.Element;
            Value = element.Value;
        }
    }
}