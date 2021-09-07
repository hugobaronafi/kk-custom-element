namespace Core.KenticoKontent.Models.Management.Elements
{
    public class MultipleChoiceElement : AbstractReferenceListElement
    {
        public const string Type = "multiple_choice";

        internal MultipleChoiceElement(AbstractReferenceListElement element)
        {
            Element = element.Element;
            Value = element.Value;
        }
    }
}