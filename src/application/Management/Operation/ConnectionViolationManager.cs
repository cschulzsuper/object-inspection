using Super.Paula.RuntimeData;

namespace Super.Paula.Application.Operation;

public class ConnectionViolationManager : IConnectionViolationManager
{
    private readonly IRuntimeCache<ConnectionViolation> _connectionViolationRuntimeCache;

    public ConnectionViolationManager(IRuntimeCache<ConnectionViolation> connectionViolationRuntimeCache)
    {
        _connectionViolationRuntimeCache=connectionViolationRuntimeCache;
    }

    public void Trace(string violator)
        => _connectionViolationRuntimeCache
            .CreateOrUpdate(
                () => new ConnectionViolation
                {
                    Violator = violator,
                    ViolationCounter = 1
                },
                connectionViolation => connectionViolation.ViolationCounter++,
                violator);

    public bool Verify(string violator)
        => _connectionViolationRuntimeCache.GetOrDefault(violator)?.ViolationCounter >= 0b_1111_1111;

}