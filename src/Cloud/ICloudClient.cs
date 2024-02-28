using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exercise1.Commands.FetchData;

namespace Exercise1.Cloud
{
    public interface ICloudClient
    {
        Task Upsert(PublicApisResponse apiResponse);
        Task Log(Log log);
        Task<(string payload, bool found)> Fetch(Guid id);
        Task<IEnumerable<Log>> FetchLogs(DateTimeOffset from, DateTimeOffset to);
    }
}