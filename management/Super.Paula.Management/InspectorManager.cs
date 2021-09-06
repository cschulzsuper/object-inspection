using Super.Paula.Data;
using Super.Paula.Aggregates.Inspectors;
using Super.Paula.Management.Contract;

namespace Super.Paula.Management
{
    public class  InspectorManager : IInspectorManager
    {
        private readonly IRepository<Inspector> _inspectorRepository;

        public InspectorManager(IRepository<Inspector> inspectorRepository)
        {
            _inspectorRepository = inspectorRepository;
        }

        public ValueTask<Inspector> GetAsync(string inspector)
            => _inspectorRepository.GetByIdAsync(inspector);

        public IQueryable<Inspector> GetQueryable()
            => _inspectorRepository.GetQueryable();

        public IAsyncEnumerable<Inspector> GetAsyncEnumerable()
            => _inspectorRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspector>, IQueryable<TResult>> query)
            => _inspectorRepository.GetAsyncEnumerable(query);

        public ValueTask InsertAsync(Inspector inspector)
        {
            if( string.IsNullOrWhiteSpace(inspector.Proof) )
            {
                inspector.Proof = $"{Guid.NewGuid()}";
            }

            return _inspectorRepository.InsertAsync(inspector);
        }

        public async ValueTask UpdateAsync(Inspector inspector)
        {
            await _inspectorRepository.UpdateAsync(inspector);
        }

        public ValueTask DeleteAsync(Inspector inspector)
            => _inspectorRepository.DeleteAsync(inspector);
    }
}