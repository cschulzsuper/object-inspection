using Super.Paula.Aggregates.Administration;
using Super.Paula.Aggregates.Guidlines;
using Super.Paula.Data;
using Super.Paula.Management.Contract;

namespace Super.Paula.Management
{
    public class  InspectionManager : IInspectionManager
    {
        private readonly IRepository<Inspection> _inspectionRepository;

        public InspectionManager(IRepository<Inspection> inspectionRepository)
        {
            _inspectionRepository = inspectionRepository;
        }

        public ValueTask<Inspection> GetAsync(string inspection)
            => _inspectionRepository.GetByIdAsync(inspection);

        public IQueryable<Inspection> GetQueryable()
            => _inspectionRepository.GetQueryable();

        public IAsyncEnumerable<Inspection> GetAsyncEnumerable()
            => _inspectionRepository.GetAsyncEnumerable();

        public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Inspection>, IQueryable<TResult>> query)
            => _inspectionRepository.GetAsyncEnumerable(query);

        public ValueTask InsertAsync(Inspection inspection)
        {
            return _inspectionRepository.InsertAsync(inspection);
        }

        public async ValueTask UpdateAsync(Inspection inspection)
        {
            await _inspectionRepository.UpdateAsync(inspection);
        }

        public ValueTask DeleteAsync(Inspection inspection)
            => _inspectionRepository.DeleteAsync(inspection);
    }
}