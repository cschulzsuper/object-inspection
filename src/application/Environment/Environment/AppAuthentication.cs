namespace Super.Paula.Environment
{
    public class AppAuthentication
    {
        public virtual string Impersonator { get; set; } = string.Empty;

        public virtual string Bearer { get; set; } = string.Empty;
    }
}
