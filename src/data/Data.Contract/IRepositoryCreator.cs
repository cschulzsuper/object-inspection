using System.Threading.Tasks;

namespace Super.Paula.Data
{
    public interface IRepositoryCreator
    {
        ValueTask CreateApplicationAsync();

        ValueTask DestroyApplicationAsync();
    }
}