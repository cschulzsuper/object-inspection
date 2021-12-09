using System.Threading.Tasks;

namespace Super.Paula.Application
{
    public interface IEventHandler<TEvent>
    {
        ValueTask ProcessAsync(string key, TEvent @event);
    }
}
