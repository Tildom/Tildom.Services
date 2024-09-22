using Azure.Storage.Blobs;

namespace Tildom.Services;

public interface IAzureBlobStorageWrapper {
  string ConnectionString { get; init; }
  BlobServiceClient ServiceClient { get; init; }
}

public class AzureBlobStorageWrapper(string connectionString) : IAzureBlobStorageWrapper {
  public string ConnectionString { get; init; } = connectionString;
  public BlobServiceClient ServiceClient { get; init; } = new BlobServiceClient(connectionString);
}
