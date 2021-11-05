using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.RuntimeData
{
    public interface IRuntimeCache<TEntity>
        where TEntity : class, IRuntimeData
    {
        TEntity? Get(params string[] correlationParts);

        void Remove(params string[] correlationParts);

        void Set(TEntity value);
    }
}
