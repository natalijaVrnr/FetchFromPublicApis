using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using FetchFromPublicApis_FunctionApp.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FetchFromPublicApis_FunctionApp.Services.TableService
{
    public class TableService : ITableService
    {
        private TableServiceClient _tableServiceClient;
        private ILogger<TableService> _logger;

        public TableService(IConfiguration config, ILogger<TableService> logger)
        {
            _tableServiceClient = new TableServiceClient(config["ConnectionStrings:StorageAccount"]);
            _logger = logger;
        }

        public async Task UploadRecord<T>(string tableName, T entity) where T : ITableEntity
        {
            try
            {
                TableClient tableClient = _tableServiceClient.GetTableClient(tableName);

                await tableClient.UpsertEntityAsync(entity);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<List<T>> FilterRecords<T>(string tableName, string query) where T : class, ITableEntity
        {
            try
            {
                TableClient tableClient = _tableServiceClient.GetTableClient(tableName);

                Pageable<T> output = tableClient.Query<T>(filter: query);

                return output.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
