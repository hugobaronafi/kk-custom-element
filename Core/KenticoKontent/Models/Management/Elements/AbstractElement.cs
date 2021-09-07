using Core.KenticoKontent.Models.Management.References;

namespace Core.KenticoKontent.Models.Management.Elements
{
    public abstract class AbstractElement<T> : IElement
    {
        public T Value { get; set; }

        public Reference? Element { get; set; }
    }
}