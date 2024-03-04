using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace FetchFromPublicApis_FunctionApp.Services.TableService
{
    public class TableService : ITableService
    {
        private TableServiceClient _tableServiceClient;

        public TableService(IConfiguration config)
        {
            _tableServiceClient = new TableServiceClient(config.GetConnectionString("StorageAccount"));
        }

        public async Task UploadRecord<T>(string tableName, T entity) where T : ITableEntity
        {
            TableClient tableClient = _tableServiceClient.GetTableClient(tableName);

            await tableClient.UpsertEntityAsync(entity);
        }
    }
}
