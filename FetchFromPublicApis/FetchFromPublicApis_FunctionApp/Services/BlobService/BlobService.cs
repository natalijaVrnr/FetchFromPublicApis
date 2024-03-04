using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FetchFromPublicApis_FunctionApp.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration config)
        {
            _blobServiceClient = new BlobServiceClient(config.GetConnectionString("StorageAccount"));
        }

        public async Task UploadBlobFromStream(string containerName, string blobName, string blobContent)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            byte[] byteArray = Encoding.UTF8.GetBytes(blobContent);

            //upload byte array as blob
            using (var stream = new MemoryStream(byteArray))
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}
