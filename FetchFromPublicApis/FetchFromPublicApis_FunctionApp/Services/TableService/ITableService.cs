using Azure.Data.Tables;
using System.Threading.Tasks;

namespace FetchFromPublicApis_FunctionApp.Services.TableService
{
    public interface ITableService
    {
        Task UploadRecord<T>(string tableName, T entity) where T : ITableEntity;
    }
}