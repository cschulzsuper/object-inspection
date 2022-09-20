using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Data;

public interface IRepositoryCreator
{
    ValueTask CreateApplicationAsync(string organization);

    ValueTask DestroyApplicationAsync(string organization);
}