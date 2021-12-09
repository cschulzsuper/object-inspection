using Super.Paula.RuntimeData;

namespace Super.Paula.Application.Runtime
{
    public class ConnectionViolation : IRuntimeData
    {
        public string Correlation => $"{Violator}";

        public string Violator { get; set; } = string.Empty;

        public int ViolationCounter { get; set; } = 0;
    }
}
