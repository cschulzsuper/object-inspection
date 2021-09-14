namespace Super.Paula.Environment
{
    public class AppSettings
    {
        public virtual string CosmosEndpoint { get; set; } = string.Empty!;

        public virtual string CosmosKey { get; set; } = string.Empty!;

        public virtual string Maintainer { get; set; } = string.Empty!;

        public virtual string MaintainerOrganization { get; set; } = string.Empty!;

        public virtual string Server { get; set; } = string.Empty!;
    }
}