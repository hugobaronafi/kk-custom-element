namespace Core.KenticoKontent.Models.Management.Elements
{
    public class AssetElement : AbstractReferenceListElement
    {
        public const string Type = "asset";

        internal AssetElement(AbstractReferenceListElement element)
        {
            Element = element.Element;
            Value = element.Value;
        }
    }
}