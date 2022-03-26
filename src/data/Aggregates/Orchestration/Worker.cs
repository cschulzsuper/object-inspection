namespace Super.Paula.Application.Orchestration
{
    public class Worker
    {
        public string ETag { get; set; } = string.Empty;

        public string UniqueName { get; set; } = string.Empty;

        public int IterationDelay { get; set; }
    }
}
