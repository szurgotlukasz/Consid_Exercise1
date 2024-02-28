using System.Collections.Generic;
using System.Linq;
using Exercise1.Cloud;

namespace Exercise1.Queries.ListAllLogs
{
    public class ListAllLogsResponse
    {
        public ListAllLogsResponse(IEnumerable<Log> logs)
        {
            Logs = logs.Select(x=> new LogDTO(x));
        }

        public IEnumerable<LogDTO> Logs { get; }
    }
}