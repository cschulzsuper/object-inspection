using System.Linq;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class OrganizationQueries : IOrganizationQueries
{
    private readonly IOrganizationManager _organizationManager;

    public OrganizationQueries(IOrganizationManager organizationManager)
    {
        _organizationManager = organizationManager;
    }

    public string[] GetAllUniqueNames()
        => _organizationManager.GetQueryable()
            .Select(x => x.UniqueName)
            .ToArray();
}