using System.Collections.Concurrent;
using System.Net;

namespace Super.Paula.Application.Administration
{
    public class ConnectionBlacklist : IConnectionBlacklist
    {
        public ConcurrentDictionary<IPAddress, int> blacklist = new ConcurrentDictionary<IPAddress, int>();

        public void Trace(IPAddress? ipAddess)
        {
            if (ipAddess != null)
            {
                blacklist.AddOrUpdate(ipAddess, 1, (_, x) => ++x);
            }
        }

        public bool Contains(IPAddress? ipAddess)
            => ipAddess != null && blacklist.TryGetValue(ipAddess, out var counter) && counter > 2;
    }
}
