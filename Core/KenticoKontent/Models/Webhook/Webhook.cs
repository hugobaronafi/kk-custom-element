using System;
using System.Collections.Generic;

using Core.KenticoKontent.Models.Management.References;

using Newtonsoft.Json;

namespace Core.KenticoKontent.Models.Webhook
{
    public class Webhook
    {
        public Data? Data { get; set; }

        public Message? Message { get; set; }

        public void Deconstruct(
            out Data data,
            out Message message
            )
        {
            data = Data ?? throw new ArgumentNullException(nameof(Data));
            message = Message ?? throw new ArgumentNullException(nameof(Message));
        }
    }

    public class Data
    {
        public IEnumerable<Item>? Items { get; set; }

        public IEnumerable<Taxonomy>? Taxonomies { get; set; }

        public void Deconstruct(
            out IEnumerable<Item> items,
            out IEnumerable<Taxonomy> taxonomies
            )
        {
            items = Items ?? throw new ArgumentNullException(nameof(Items));
            taxonomies = Taxonomies ?? throw new ArgumentNullException(nameof(Taxonomies));
        }
    }

    public class Item
    {
        [JsonProperty("item")]
        public Reference? ItemReference { get; set; }

        [JsonProperty("language")]
        public Reference? LanguageReference { get; set; }

        [JsonProperty("transition_from")]
        public Reference? TransitionFromReference { get; set; }

        [JsonProperty("transition_to")]
        public Reference? TransitionToReference { get; set; }
    }

    public class Taxonomy
    {
        public string? Codename { get; set; }
    }

    public class Message
    {
        public Guid Id { get; set; }

        public string? Type { get; set; }

        public string? Operation { get; set; }

        [JsonProperty("api_name")]
        public string? ApiName { get; set; }

        [JsonProperty("project_id")]
        public Guid ProjectId { get; set; }
    }
}