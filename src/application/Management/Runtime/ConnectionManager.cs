using Super.Paula.RuntimeData;

namespace Super.Paula.Application.Runtime
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly IRuntimeCache<Connection> _connectionRuntimeCache;

        public ConnectionManager(IRuntimeCache<Connection> connectionRuntimeCache)
        {
            _connectionRuntimeCache = connectionRuntimeCache;
        }

        public void Trace(string account, string proof)
        {
            var connection = _connectionRuntimeCache.Get(account)
                ?? new Connection
                {
                    Account = account
                };

            connection.Proof.Add(proof);

            _connectionRuntimeCache.Set(connection);
        }

        public void Forget(string account)
            => _connectionRuntimeCache.Remove(account);

        public bool Verify(string account, string proof)
            => _connectionRuntimeCache.Get(account)?.Proof.Contains(proof) == true;

    }
}
