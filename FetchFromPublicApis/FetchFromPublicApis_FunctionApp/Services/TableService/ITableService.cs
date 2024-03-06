using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FetchFromPublicApis_FunctionApp.Services.TableService
{
    public interface ITableService
    {
        Task UploadRecord<T>(string tableName, T entity) where T : ITableEntity;
        Task<List<T>> FilterRecords<T>(string tableName, string query) where T : class, ITableEntity;
    }
}