using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventProcessingManager
    {
        ValueTask<EventProcessing> GetAsync(string id);
        IQueryable<EventProcessing> GetQueryable();
        IAsyncEnumerable<EventProcessing> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<EventProcessing>, IQueryable<TResult>> query);
        ValueTask InsertAsync(EventProcessing eventProcessing);
        ValueTask UpdateAsync(EventProcessing eventProcessing);
        ValueTask DeleteAsync(EventProcessing eventProcessing);
    }
}