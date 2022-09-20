using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IExtensionManager
{
    Extension Get(string extension);
    ValueTask<Extension> GetAsync(string extension);
    IQueryable<Extension> GetQueryable();

    IAsyncEnumerable<Extension> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Extension>, IQueryable<TResult>> query);

    ValueTask InsertAsync(Extension extension);
    ValueTask UpdateAsync(Extension extension);
    ValueTask DeleteAsync(Extension extension);
}