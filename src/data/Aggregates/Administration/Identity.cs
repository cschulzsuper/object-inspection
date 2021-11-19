using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class Identity
    {
        public string UniqueName { get; set; } = string.Empty;
        public string MailAddress { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
