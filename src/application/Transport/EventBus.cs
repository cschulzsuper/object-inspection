using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Inventory;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public EventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TEvent>(string category, string key, TEvent @event)
        {
            switch(category)
            {
                case EventCategories.BusinessObject:
                    var eventHandler = _serviceProvider.GetRequiredService<IBusinessObjectEventHandler>();
                    if (eventHandler is IEventHandler<TEvent> typedEventHandler )
                    {
                        await typedEventHandler.ProcessAsync(key, @event);
                    }

                    break;
            }
        }
    }
}
