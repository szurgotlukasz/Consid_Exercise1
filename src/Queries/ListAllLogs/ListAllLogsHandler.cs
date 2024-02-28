using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Exercise1.Cloud;
using MediatR;

namespace Exercise1.Queries.ListAllLogs
{
    public class ListAllLogsHandler : IRequestHandler<ListAllLogsQuery, ListAllLogsResponse>
    {
        private readonly ICloudClient cloudClient;

        public ListAllLogsHandler(ICloudClient cloudClient)
        {
            this.cloudClient = cloudClient;
        }
        public async Task<ListAllLogsResponse> Handle(ListAllLogsQuery request, CancellationToken cancellationToken)
        {
            var logs = await cloudClient.FetchLogs(request.From, request.To);
            return new ListAllLogsResponse(logs);
        }
    }
}

