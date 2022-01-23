namespace Super.Paula.Environment
{
    public class AppSettings
    {
        public string StreamerSecret { get; set; } = string.Empty;

        public string CosmosEndpoint { get; set; } = string.Empty;

        public string CosmosKey { get; set; } = string.Empty;

        public string CosmosDatabase { get; set; } = string.Empty;

        public string Maintainer { get; set; } = string.Empty;

        public string MaintainerOrganization { get; set; } = string.Empty;

        public string Server { get; set; } = string.Empty;
    }
}