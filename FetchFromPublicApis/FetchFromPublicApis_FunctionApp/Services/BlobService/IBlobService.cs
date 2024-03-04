using System.Threading.Tasks;

namespace FetchFromPublicApis_FunctionApp.Services.BlobService
{
    public interface IBlobService
    {
        Task UploadBlobFromStream(string containerName, string blobName, string blobContent);
    }
}