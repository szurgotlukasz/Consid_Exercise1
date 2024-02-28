using System;
using Azure;
using Azure.Data.Tables;

namespace Exercise1.Cloud
{
    public class LogEntity : ITableEntity
    {
        public LogEntity() { }

        public LogEntity(Log log, string partitionKey)
        {
            PartitionKey = partitionKey;
            Status = log.Status;
            LogTimestamp = log.Timestamp;
            RowKey = GenerateRowKey(LogTimestamp);
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public Status Status { get; set; }
        public DateTimeOffset LogTimestamp { get; set; }

        public static LogEntity FromLog(Log log, string partitionKey)
        {
            return new LogEntity(log, partitionKey)
            {
                LogTimestamp = log.Timestamp
            };
        }

        public Log ToLog()
        {
            return new Log(this.Status, this.LogTimestamp);
        }

        public static string GenerateRowKey(DateTimeOffset timestamp)
        {
            return timestamp.ToString("yyyyMMddHHmmss");
        }

    }
}

