using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchFromPublicApis_FunctionApp.Entities
{
    public class LogRecordEntity: ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public LogRecordEntity() : base()
        {
        }

        public LogRecordEntity(string content, DateTime requestDateTime)
        {
            PartitionKey = requestDateTime.ToString("dd-MM-yyyy");
            RowKey = requestDateTime.ToString("dd-MM-yyyyTHH:mm:ss");
            Content = content;
            Timestamp = DateTime.UtcNow;
        }
    }
}
