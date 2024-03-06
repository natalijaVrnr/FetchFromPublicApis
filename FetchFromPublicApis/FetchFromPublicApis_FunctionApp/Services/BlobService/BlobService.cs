using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FetchFromPublicApis_FunctionApp.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private BlobServiceClient _blobServiceClient;
        private ILogger<BlobService> _logger;

        public BlobService(IConfiguration config, ILogger<BlobService> logger)
        {
            _blobServiceClient = new BlobServiceClient(config["ConnectionStrings:StorageAccount"]);
            _logger = logger;
        }

        public async Task UploadBlobFromStream(string containerName, string blobName, string blobContent)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<string> GetBlobContent(string containerName, string blobName)
        {
            try
            {
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

                using (StreamReader reader = new StreamReader(blobDownloadInfo.Content))
                {
                    string content = await reader.ReadToEndAsync();
                    return content;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
