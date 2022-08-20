using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication;

public interface INotificationManager
{
    ValueTask<Notification> GetAsync(string inspector, int date, int time);

    IQueryable<Notification> GetQueryable();
    IAsyncEnumerable<Notification> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Notification>, IQueryable<TResult>> query);

    IQueryable<Notification> GetInspectorBasedQueryable(string inspector);
    IAsyncEnumerable<Notification> GetInspectorBasedAsyncEnumerable(string inspector);
    IAsyncEnumerable<TResult> GetInspectorBasedAsyncEnumerable<TResult>(string inspector, Func<IQueryable<Notification>, IQueryable<TResult>> query);


    ValueTask InsertAsync(Notification notification);
    ValueTask UpdateAsync(Notification notification);
    ValueTask DeleteAsync(Notification notification);
}