﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public interface IBusinessObjectManager
{
    ValueTask<BusinessObject> GetAsync(string businessObject);
    IQueryable<BusinessObject> GetQueryable();

    IAsyncEnumerable<BusinessObject> GetAsyncEnumerable();
    IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<BusinessObject>, IQueryable<TResult>> query);

    ValueTask InsertAsync(BusinessObject businessObject);
    ValueTask UpdateAsync(BusinessObject businessObject);
    ValueTask DeleteAsync(BusinessObject businessObject);
}