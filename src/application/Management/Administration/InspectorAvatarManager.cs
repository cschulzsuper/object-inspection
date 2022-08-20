using Super.Paula.Application.Administration.Exceptions;
using Super.Paula.BlobStorage;
using Super.Paula.BlobStorage.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public class InspectorAvatarManager : IInspectorAvatarManager
{
    private readonly IBlobStorage _blobStorage;

    public InspectorAvatarManager(IBlobStorage blobStorage)
    {
        _blobStorage = blobStorage;
    }

    public async ValueTask<Stream> GetAsync(string inspector)
    {
        try
        {
            return await _blobStorage.ReadAsync($"inspector-avatars/{inspector}");
        }
        catch (BlobNotFoundException exception)
        {
            throw new InspectorAvatarNotFoundException($"Could not find inspector avatar '{inspector}' in blob storage.", exception);
        }
        catch (Exception exception)
        {
            throw new InspectorAvatarNotFoundException($"Unexpected error while fetching inspector avatar '{inspector}' from blob storage.", exception);
        }
    }

    public async ValueTask DeleteAsync(string inspector)
        => await _blobStorage.RemoveAsync($"inspector-avatars/{inspector}");

    public async ValueTask SetAsync(Stream stream, string inspector)
        => await _blobStorage.WriteOrReplaceAsync(new InspectorAvatarStream(stream), $"inspector-avatars/{inspector}");

}