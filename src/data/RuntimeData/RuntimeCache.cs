using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Super.Paula.RuntimeData
{
    public class RuntimeCache<TEntity> : IRuntimeCache<TEntity>
        where TEntity : class, IRuntimeData
    {
        private readonly ConcurrentDictionary<string, TEntity> _runtimeBucket = new ConcurrentDictionary<string, TEntity>();

        public TEntity? Get(params string[] correlationParts)
        {
            var correlation = string.Join(":", correlationParts);
            
            return _runtimeBucket.ContainsKey(correlation)
                ? _runtimeBucket[correlation]
                : null;
        }

        public void Remove(params string[] correlationParts)
        {
            var correlation = string.Join(":", correlationParts);

            if (_runtimeBucket.ContainsKey(correlation))
            {
                _runtimeBucket.Remove(correlation, out _);
            }
        }

        public void Set(TEntity value)
        {
            _runtimeBucket[value.Correlation] = value;
        }
    }
}
