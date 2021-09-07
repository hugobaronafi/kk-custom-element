using System;
using System.Threading.Tasks;

using Core.Gatsby.Models;
using Core.KenticoKontent.Models.Webhook;

namespace Core.Gatsby.Services
{
    public interface IGatsbyProxy
    {
        Task ForwardWebhook(Webhook webhook, string gatsbyWebhook);

        Action<T1> Debounce<T1>(Action<DebounceMode, T1> function, TimeSpan wait, DebounceMode mode = DebounceMode.Trailing);
    }
}