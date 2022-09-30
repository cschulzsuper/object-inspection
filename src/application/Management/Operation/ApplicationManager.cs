using ChristianSchulz.ObjectInspection.Data;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public class ApplicationManager : IApplicationManager
{
    private readonly IRepositoryCreator _repositoryCreator;

    public ApplicationManager(IRepositoryCreator repositoryCreator)
    {
        _repositoryCreator = repositoryCreator;
    }

    public ValueTask InitializeAsync(string organization)
        => _repositoryCreator.CreateApplicationAsync(organization);

    public ValueTask PrugeAsync(string organization)
        => _repositoryCreator.DestroyApplicationAsync(organization);
}