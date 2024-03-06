using FetchFromPublicApis_FunctionApp.Entities;
using FetchFromPublicApis_FunctionApp.Services.BlobService;
using FetchFromPublicApis_FunctionApp.Services.TableService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FetchFromPublicApis_WebApi.Controllers
{
    [Route("api/blobs")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private IBlobService _blobService;
        private readonly ILogger<LogController> _logger;
        public BlobController(IBlobService blobService, ILogger<LogController> logger)
        {
            _blobService = blobService;
            _logger = logger;
        }

        /// <summary>
        /// Get blob data by log entry row key
        /// </summary>
        /// <param name="rowKey" example="31-03-2024T18:34:00">Log row key as dd-MM-yyyyTHH:mm:ss</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBlobDataByLogRowKey(string rowKey)
        {
            try
            {
                if (!DateTime.TryParseExact(rowKey, "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFromDate))
                {
                    throw new Exception("Please enter rowKey in a correct format dd-MM-yyyyTHH:mm:ss");
                }

                _logger.LogInformation("GetBlobDataByLogRowKey started processing request");
                _logger.LogInformation($"Retrieved parameters - rowKey: {rowKey}");

                string blobName = $"{rowKey.Substring(0, rowKey.Length - 9)}/{rowKey}.json";
                string blobContent = await _blobService.GetBlobContent("data", blobName);

                return Ok(blobContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
