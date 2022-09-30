using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using ChristianSchulz.ObjectInspection.BlobStorage.Exceptions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.BlobStorage;

public class BlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorage(IAzureClientFactory<BlobServiceClient> blobClientFactory)
    {
        _blobServiceClient = blobClientFactory.CreateClient("ObjectInspectionBlobStorage");
    }

    public async ValueTask<Stream> ReadAsync(string path)
    {
        try
        {
            var blobClient = CreateBlobClient(path);

            return await blobClient.OpenReadAsync();
        }
        catch (RequestFailedException exception) when (exception.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            throw new BlobNotFoundException($"Could not find blob in storage.", exception);
        }
    }

    public async ValueTask RemoveAsync(string path)
    {
        var blobClient = CreateBlobClient(path);

        await blobClient.DeleteIfExistsAsync();
    }

    public async ValueTask RemoveAsync(string path, string btag)
    {
        var blobClient = CreateBlobClient(path);

        await blobClient.DeleteIfExistsAsync(
            conditions: new BlobRequestConditions()
            {
                IfMatch = new ETag(btag)
            });
    }

    public async ValueTask<string> WriteOrReplaceAsync(Stream stream, string path)
    {
        var blobClient = CreateBlobClient(path);

        var response = await blobClient.UploadAsync(stream, true);

        return response.Value.ETag
            .ToString()
            .Trim('"');
    }

    public async ValueTask<string> ReplaceAsync(Stream stream, string path, string btag)
    {
        var blobClient = CreateBlobClient(path);

        var response = await blobClient.UploadAsync(stream,
            options: new BlobUploadOptions()
            {
                Conditions = new BlobRequestConditions()
                {
                    IfMatch = new ETag(btag)
                }
            });

        return response.Value.ETag
            .ToString()
            .Trim('"');
    }

    public async ValueTask<string> WriteAsync(Stream stream, string path)
    {
        var blobClient = CreateBlobClient(path);

        var response = await blobClient.UploadAsync(stream);

        return response.Value.ETag
            .ToString()
            .Trim('"');
    }

    private BlobClient CreateBlobClient(string path)
    {
        var blobContainerName = path
            .Split('/', 2)
            .FirstOrDefault();

        if (blobContainerName == null)
        {
            throw new BlobStorageException($"Blob path '{path}' has no directory.");
        }

        if (blobContainerName == string.Empty)
        {
            throw new BlobStorageException($"Blob path '{path}' can't be rooted no directory.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);

        blobContainerClient.CreateIfNotExists();

        var blobName = Path.GetRelativePath(blobContainerName, path);
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        return blobClient;
    }
}