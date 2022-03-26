using Super.Paula.RuntimeData;

namespace Super.Paula.Application.Orchestration
{
    public class WorkerRuntime : IRuntimeData
    {
        public string Correlation => Worker;

        public string Worker { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public int Date { get; set; }

        public int Time { get; set; }
    }
}
