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

        public void Trace(string realm, string account, string proof)
        {
            var connection = _connectionRuntimeCache.Get(realm, account)
                ?? new Connection
                {
                    Realm = realm,
                    Account = account
                };

            connection.Proof.Add(proof);

            _connectionRuntimeCache.Set(connection);
        }

        public void Forget(string realm, string account)
            => _connectionRuntimeCache.Remove(realm, account);

        public bool Verify(string realm, string account, string proof)
            => _connectionRuntimeCache.Get(realm, account)?.Proof.Contains(proof) == true;

    }
}
