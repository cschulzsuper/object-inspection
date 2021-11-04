using System.Net;

namespace Super.Paula.Application.Administration
{
    public interface IConnectionBlacklist
    {
        void Trace(IPAddress? ipAddess);

        bool Contains(IPAddress? ipAddess);
    }
}
