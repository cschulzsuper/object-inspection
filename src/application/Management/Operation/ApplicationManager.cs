using Super.Paula.Data;
using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IRepositoryCreator _repositoryCreator;

        public ApplicationManager(IRepositoryCreator repositoryCreator)
        {
            _repositoryCreator = repositoryCreator;
        }

        public ValueTask InitializeAsync()
            => _repositoryCreator.CreateApplicationAsync();

        public ValueTask PrugeAsync()
            => _repositoryCreator.DestroyApplicationAsync();

    }
}
