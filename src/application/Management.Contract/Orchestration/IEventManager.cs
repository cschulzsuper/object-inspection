using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventManager
    {
        ValueTask<Event> GetAsync(string id);
        IQueryable<Event> GetQueryable();
        IAsyncEnumerable<Event> GetAsyncEnumerable();
        IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Event>, IQueryable<TResult>> query);
        ValueTask InsertAsync(Event @event);
        ValueTask UpdateAsync(Event @event);
        ValueTask DeleteAsync(Event @event);
    }
}