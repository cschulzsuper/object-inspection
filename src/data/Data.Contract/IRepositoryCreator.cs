using System.Threading.Tasks;

namespace Super.Paula.Data
{
    public interface IRepositoryCreator
    {
        ValueTask CreateApplicationAsync(string organization);

        ValueTask DestroyApplicationAsync(string organization);
    }
}