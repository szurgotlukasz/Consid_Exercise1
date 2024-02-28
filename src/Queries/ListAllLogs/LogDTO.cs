using System;
using Exercise1.Cloud;

namespace Exercise1.Queries.ListAllLogs
{
    public class LogDTO
    {
        public LogDTO(Log x)
        {
            this.Status = Enum.GetName(typeof(Status), x.Status);
            this.Timestamp = x.Timestamp;
        }

        public string Status { get; }
        public DateTimeOffset Timestamp { get; }
    }
}

