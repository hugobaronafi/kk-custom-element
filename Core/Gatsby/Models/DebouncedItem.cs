using Core.KenticoKontent.Models.Webhook;

namespace Core.Gatsby.Models
{
    public class DebouncedItem
    {
        public Webhook Webhook { get; set; }

        public string GatsbyWebhook { get; set; }

        public DebouncedItem(Webhook webhook, string gatsbyWebhook)
        {
            Webhook = webhook;
            GatsbyWebhook = gatsbyWebhook;
        }
    }
}