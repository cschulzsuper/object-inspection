using System;

namespace Super.Paula.Environment
{
    public class AppAuthentication
    {
        public string Token { get; set; } = string.Empty;

        public long Ticks { get; set; } = 0;

        public string Organization { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;
        public string Proof { get; set; } = string.Empty;

        public string ImpersonatorOrganization { get; set; } = string.Empty;
        public string ImpersonatorInspector { get; set; } = string.Empty;

        public string[] Authorizations { get; set; } = Array.Empty<string>();

        public string[] AuthorizationsFilter { get; set; } = Array.Empty<string>();

    }
}
