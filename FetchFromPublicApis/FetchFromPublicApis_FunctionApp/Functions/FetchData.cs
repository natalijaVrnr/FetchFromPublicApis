using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Net.Http;
using FetchFromPublicApis_FunctionApp.Services.BlobService;
using FetchFromPublicApis_FunctionApp.Services.TableService;
using FetchFromPublicApis_FunctionApp.Entities;
using Microsoft.Extensions.Logging;

namespace FetchFromPublicApis_FunctionApp.Functions
{
    public class FetchData
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _apiUrl = "https://api.publicapis.org/random?auth=null";
        private IBlobService _blobService;
        private ITableService _tableService;
        private ILogger<FetchData> _logger;

        public FetchData(IBlobService blobService, ITableService tableService, ILogger<FetchData> logger)
        {
            _blobService = blobService;
            _tableService = tableService;
            _logger = logger;
        }

        [FunctionName("FetchData")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timer)
        {
            HttpResponseMessage response = new();
            try
            {
                _logger.LogInformation($"Sending request to {_apiUrl}");
                response = await _httpClient.GetAsync(_apiUrl);
                _logger.LogInformation($"Received response: {response.ToString()}");

                DateTime requestDateTime = response.Headers.Date.Value.DateTime;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"Uploading response to {requestDateTime.ToString("yyyy-MM-dd")}/{requestDateTime.ToString("yyyy-MM-ddTHH:mm:ss")}.txt");
                    await _blobService.UploadBlobFromStream("data",
                        $"{requestDateTime.ToString("yyyy-MM-dd")}/{requestDateTime.ToString("yyyy-MM-ddTHH:mm:ss")}.txt", responseBody);
                    _logger.LogInformation($"Uploaded to blob {requestDateTime.ToString("yyyy-MM-dd")}/{requestDateTime.ToString("yyyy-MM-ddTHH:mm:ss")}.txt");
                    
                    _logger.LogInformation($"Uploading log to success table");
                    var logRecordEntity = new LogRecordEntity(response.ToString(), requestDateTime);
                    await _tableService.UploadRecord("success", logRecordEntity);
                    _logger.LogInformation($"Record in success table created successfully: entity key {logRecordEntity.PartitionKey}, row key {logRecordEntity.RowKey}");
                }

                else
                {
                    _logger.LogInformation($"Uploading log to fail table");
                    var logRecordEntity = new LogRecordEntity(response.ToString(), requestDateTime);
                    await _tableService.UploadRecord("fail", logRecordEntity);
                    _logger.LogInformation($"Record in fail table created successfully: entity key {logRecordEntity.PartitionKey}, row key {logRecordEntity.RowKey}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
