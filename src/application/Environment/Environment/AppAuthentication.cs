using System;

namespace Super.Paula.Environment
{
    public class AppAuthentication
    {
        public virtual long Ticks { get; set; } = 0;

        public virtual string Bearer { get; set; } = string.Empty;

        public virtual string Inspector { get; set; } = string.Empty;

        public virtual string Organization { get; set; } = string.Empty;

        public virtual string ImpersonatorBearer { get; set; } = string.Empty;

        public virtual string ImpersonatorInspector { get; set; } = string.Empty;

        public virtual string ImpersonatorOrganization { get; set; } = string.Empty;

        public virtual string[] Authorizations { get; set; } = Array.Empty<string>();

        public virtual string[] AuthorizationsFilter { get; set; } = Array.Empty<string>();
    }
}
