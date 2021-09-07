using Core.KenticoKontent.Models.Management.References;

using Newtonsoft.Json;

namespace Core.KenticoKontent.Models.Management.Elements
{
    [JsonConverter(typeof(ElementTypeResolver))]
    public interface IElement
    {
        public Reference? Element { get; set; }
    }
}