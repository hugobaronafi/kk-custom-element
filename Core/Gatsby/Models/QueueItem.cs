using Core.KenticoKontent.Models.Webhook;

namespace Core.Gatsby.Models
{
    public class QueueItem
    {
        public Webhook Webhook { get; set; }

        public string GatsbyWebhook { get; set; }

        public QueueItem(Webhook webhook, string gatsbyWebhook)
        {
            Webhook = webhook;
            GatsbyWebhook = gatsbyWebhook;
        }
    }
}