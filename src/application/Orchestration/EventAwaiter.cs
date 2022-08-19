using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class EventAwaiter
    {
        public TaskCompletionSource? _tcs;
        public bool _wasSignaled = false;
        
        public void Signal()
        {
            if (_tcs != null)
            {
                _tcs.SetResult();
                _tcs = null;
            } 
            else
            {
                _wasSignaled = true;
            }
        }

        public Task WaitAsync()
        {
            if (_wasSignaled)
            {
                _wasSignaled = false;
                return Task.CompletedTask;
            }

            _tcs ??= new TaskCompletionSource();
            return _tcs.Task;
        }
    }
}
