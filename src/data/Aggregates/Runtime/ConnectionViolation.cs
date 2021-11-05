using Super.Paula.RuntimeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Runtime
{
    public class ConnectionViolation : IRuntimeData
    {
        public string Correlation => $"{Violator}";

        public string Violator { get; set; } = string.Empty;

        public int ViolationCounter { get; set; } = 0;
    }
}
