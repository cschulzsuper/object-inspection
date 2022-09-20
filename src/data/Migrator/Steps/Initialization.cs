using ChristianSchulz.ObjectInspection.Data;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Migrator.Steps;

public class Initialization : IStep
{
    private readonly AdministrationContext _administrationContext;

    public Initialization(AdministrationContext administrationContext)
    {
        _administrationContext = administrationContext;
    }

    public Task ExecuteAsync()
    {
        _administrationContext.Database.EnsureCreated();

        return Task.CompletedTask;
    }
}