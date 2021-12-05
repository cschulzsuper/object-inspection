using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(string category, string key, TEvent @event);
    }
}
