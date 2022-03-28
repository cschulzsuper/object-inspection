using Super.Paula.Application.Runtime;
using Super.Paula.RuntimeData;

namespace Super.Paula.Application.Operation
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly IRuntimeCache<Connection> _connectionRuntimeCache;

        public ConnectionManager(IRuntimeCache<Connection> connectionRuntimeCache)
        {
            _connectionRuntimeCache = connectionRuntimeCache;
        }

        public void Trace(string account, string proof)
            => _connectionRuntimeCache
                .CreateOrUpdate(() =>
                    {
                        var connection = new Connection
                        {
                            Account = account,
                        };

                        connection.Proof.Add(proof);

                        return connection;
                    },
                    connection => connection.Proof.Add(proof),
                    account);

        public void Forget(string account)
            => _connectionRuntimeCache.Remove(account);

        public bool Verify(string account, string proof)
            => _connectionRuntimeCache.GetOrDefault(account)?.Proof.Contains(proof) == true;

    }
}
