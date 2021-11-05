using System.Net;

namespace Super.Paula.Application.Runtime
{
    public interface IConnectionViolationManager
    {
        void Trace(string violator);

        bool Verify(string violator);
    }
}
