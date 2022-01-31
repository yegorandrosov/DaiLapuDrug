using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DaiLapuDrug.Web.Services
{
    public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        private async Task<BlobContainerClient> GetContainerClient(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            return containerClient;
        }

        private async Task<BlobClient> GetBlobClient(string containerName, string blobName)
        {
            var container = await GetContainerClient(containerName);

            var blobClient = container.GetBlobClient(blobName);

            return blobClient;
        }

        public async Task<string> UploadFile(string containerName, string blobName, Stream stream, string mimeType = null)
        {
            var blobClient = await GetBlobClient(containerName, blobName);
            BlobUploadOptions uploadOptions = null;

            if (mimeType != null)
            {
                uploadOptions = new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders()
                    {
                        ContentType = mimeType
                    }
                };
            }

            await blobClient.UploadAsync(stream, uploadOptions);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadFile(string containerName, string blobName)
        {
            var blobClient = await GetBlobClient(containerName, blobName);
            var ms = new MemoryStream();

            blobClient.DownloadTo(ms);
            ms.Position = 0;

            return ms;
        }

        public async Task<Uri> GetBlobUri(string containerName, string blobName)
        {
            var blobClient = await GetBlobClient(containerName, blobName);

            return blobClient.Uri;
        }

        public async Task<Uri> GetServiceSasUriForBlob(string containerName, string blobName, string storedPolicyName = null)
        {
            var blobClient = await GetBlobClient(containerName, blobName);

            if (blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(10);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                        BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

                return sasUri;
            }
            else
            {
                return null;
            }
        }
    }
}
