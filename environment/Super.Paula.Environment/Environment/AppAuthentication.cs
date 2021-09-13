using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Environment
{
    public class AppAuthentication
    {
        public virtual string Impersonator { get; set; } = string.Empty;

        public virtual string Bearer { get; set; } = string.Empty;
    }
}
