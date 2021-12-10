using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;

namespace Super.Paula.Application
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDictionary<string, Type> _eventHandlerTypes = new Dictionary<string, Type>
        {
            [EventCategories.BusinessObject] = typeof(INotificationEventHandler),
            [EventCategories.BusinessObjectInspectionAudit] = typeof(IBusinessObjectInspectionAuditEventHandler),
            [EventCategories.Notification] = typeof(INotificationEventHandler)
        };

        public EventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TEvent>(string category, string key, TEvent @event)
        {
            if (_eventHandlerTypes.ContainsKey(category))
            {
                var eventHandlerType = _eventHandlerTypes[category];
                var eventHandler = _serviceProvider.GetRequiredService(eventHandlerType);
                if (eventHandler is IEventHandler<TEvent> typedEventHandler)
                {
                    await typedEventHandler.ProcessAsync(key, @event);
                }
            }
        }
    }
}
