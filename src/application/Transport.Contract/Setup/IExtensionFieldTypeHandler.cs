﻿using System.Collections.Generic;

namespace Super.Paula.Application.Setup
{
    public interface IExtensionFieldTypeHandler
    {
        IAsyncEnumerable<string> GetAll();
    }
}