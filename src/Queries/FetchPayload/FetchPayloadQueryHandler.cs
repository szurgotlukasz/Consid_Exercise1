using System.Threading;
using System.Threading.Tasks;
using Exercise1.Cloud;
using MediatR;

namespace Exercise1.Queries
{
    public class FetchPayloadQueryHandler : IRequestHandler<FetchPayloadQuery, FetchPayloadQueryResponse>
    {
        private readonly ICloudClient _cloudClient;

        public FetchPayloadQueryHandler(ICloudClient cloudClient)
        {
            _cloudClient = cloudClient;
        }

        public async Task<FetchPayloadQueryResponse> Handle(FetchPayloadQuery request, CancellationToken cancellationToken)
        {
            var fetchResult = await _cloudClient.Fetch(request.Id);
            return new FetchPayloadQueryResponse(fetchResult.payload, fetchResult.found);
        }
    }
}

