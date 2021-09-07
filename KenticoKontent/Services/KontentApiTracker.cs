using Core.KenticoKontent.Services;

namespace KenticoKontent.Services
{
    public class KontentApiTracker : IKontentApiTracker
    {
        public int ApiCalls { get; set; }
    }
}