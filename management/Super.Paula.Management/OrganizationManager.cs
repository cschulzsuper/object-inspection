using Super.Paula.Data;
using Super.Paula.Aggregates.Organizations;
using Super.Paula.Management.Contract;

namespace Super.Paula.Management
{
    public class  OrganizationManager : IOrganizationManager
    {
        private readonly IRepository<Organization> _organizationRepository;

        public OrganizationManager(IRepository<Organization> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public ValueTask<Organization> GetAsync(string organization)
            => _organizationRepository.GetByIdAsync(organization);

        public IQueryable<Organization> GetAll()
            => _organizationRepository.GetQueryable();

        public ValueTask InsertAsync(Organization organization)
            => _organizationRepository.InsertAsync(organization);

        public ValueTask UpdateAsync(Organization organization)
            => _organizationRepository.UpdateAsync(organization);

        public ValueTask DeleteAsync(Organization organization)
            => _organizationRepository.DeleteAsync(organization);

        public IQueryable<Organization> GetQueryable()
            => _organizationRepository.GetQueryable();

        public IAsyncEnumerable<Organization> GetAsyncEnumerable()
            => _organizationRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Organization>, IQueryable<TResult>> query)
            => _organizationRepository.GetAsyncEnumerable(query);
    }
}