using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Authorization;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Data.Steps
{
    public class Reset : IStep
    {
        private readonly PaulaAdministrationContext _paulaAdministrationContext;

        public Reset(PaulaAdministrationContext paulaAdministrationContext)
        {
            _paulaAdministrationContext = paulaAdministrationContext;
        }

        public Task ExecuteAsync()
        {
            _paulaAdministrationContext.Database.EnsureDeleted();
            _paulaAdministrationContext.Database.EnsureCreated();

            return Task.CompletedTask;
        }
    }
}
