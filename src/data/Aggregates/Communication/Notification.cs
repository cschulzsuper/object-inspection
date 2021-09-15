using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class Notification
    {
        public string Inspector { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Date { get; set; }
        public int Time { get; set; }
    }
}
