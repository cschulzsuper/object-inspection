namespace Super.Paula.Application.Operation
{
    public interface IConnectionManager
    {
        void Trace(string account, string proof);

        void Forget(string account);

        bool Verify(string account, string proof);
    }
}
