using System.Linq;

namespace Super.Paula.Application.Administration
{
    public class Organizations : IOrganizations
    {
        private readonly IOrganizationManager _organizationManager;

        public Organizations(IOrganizationManager organizationManager)
        {
            _organizationManager = organizationManager;
        }

        public string[] GetAllUniqueNames()
            => _organizationManager.GetQueryable()
                .Select(x => x.UniqueName)
                .ToArray();
    }
}
