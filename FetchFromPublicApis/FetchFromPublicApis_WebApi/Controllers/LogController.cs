using FetchFromPublicApis_FunctionApp.Entities;
using FetchFromPublicApis_FunctionApp.Services.TableService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FetchFromPublicApis_WebApi.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private ITableService _tableService;
        private readonly ILogger<LogController> _logger;
        public LogController(ITableService tableService, ILogger<LogController> logger)
        {
            _tableService = tableService;
            _logger = logger;
        }

        /// <summary>
        /// Get logs for a specific time period (from/to)
        /// </summary>
        /// <param name="dateFrom" example="31-03-2024T18:34:00">Start date as dd-MM-yyyyTHH:mm:ss</param>
        /// <param name="dateTo" example="31-03-2024T20:34:00">End date as dd-MM-yyyyTHH:mm:ss</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLogsDataByTimeInterval(string dateFrom, string dateTo) 
        {
            try
            {
                if (!DateTime.TryParseExact(dateFrom, "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFromDate) 
                    || !DateTime.TryParseExact(dateTo, "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedToDate))
                {
                    throw new Exception("Please enter dateFrom and dateTo in a correct format dd-MM-yyyyTHH:mm:ss");
                }
                    
                _logger.LogInformation("GetLogsDataByTimeInterval started processing request");
                _logger.LogInformation($"Retrieved parameters - dateFrom: {dateFrom}, dateTo: {dateTo}");

                string query = $"RowKey ge '{dateFrom}' and RowKey le '{dateTo}'";

                List<LogRecordEntity> filteredSuccessLogs = await _tableService.FilterRecords<LogRecordEntity>("success", query);
                List<LogRecordEntity> filteredFailLogs = await _tableService.FilterRecords<LogRecordEntity>("fail", query);

                List<LogRecordEntity> filteredLogs = filteredSuccessLogs.Concat(filteredFailLogs).OrderBy(l => l.RowKey).ToList();

                return Ok(filteredLogs);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}
