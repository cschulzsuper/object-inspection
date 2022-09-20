using System.Linq;
using ChristianSchulz.ObjectInspection.RuntimeData;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class ConnectionManager : IConnectionManager
{
    private readonly IRuntimeCache<Connection> _connectionRuntimeCache;

    public ConnectionManager(IRuntimeCache<Connection> connectionRuntimeCache)
    {
        _connectionRuntimeCache = connectionRuntimeCache;
    }

    public void Trace(string account, string proof, string proofType)
        => _connectionRuntimeCache
            .CreateOrUpdate(() =>
                {
                    var connection = new Connection
                    {
                        Account = account,
                    };

                    AddConnectionProof(connection, proof, proofType);

                    return connection;
                },
                connection =>
                {
                    AddConnectionProof(connection, proof, proofType);
                },
                account);

    private static void AddConnectionProof(Connection connection, string proof, string proofType)
    {
        var connectionProof = new ConnectionProof
        {
            Proof = proof,
            ProofType = proofType
        };

        connection.Proof.Add(connectionProof);
    }

    public void Forget(string account)
        => _connectionRuntimeCache.Remove(account);

    public bool Verify(string account, string proof, string proofType)
        => _connectionRuntimeCache.GetOrDefault(account)?.Proof
            .Any(x => 
                x.Proof == proof && 
                x.ProofType == proofType) == true;

}