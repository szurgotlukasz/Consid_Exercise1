using System;
using System.Threading;
using System.Threading.Tasks;
using Exercise1.Cloud;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Exercise1.Commands.FetchData
{
    public class FetchDataCommandHandler : IRequestHandler<FetchDataCommand>
    {
        private readonly IPublicApisClient _publicApisClient;
        private readonly ICloudClient _tableClient;
        private readonly ISystemTimeProvider _systemTimeProvider;
        private readonly ILogger _log;

        public FetchDataCommandHandler(
            IPublicApisClient publicApisClient,
            ICloudClient tableClient,
            ISystemTimeProvider systemTimeProvider,
            ILogger<FetchDataCommandHandler> log)
        {
            _publicApisClient = publicApisClient;
            _tableClient = tableClient;
            _systemTimeProvider = systemTimeProvider;
            _log = log;
        }

        public async Task<Unit> Handle(FetchDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _publicApisClient.GetAsync();
                await _tableClient.Log(new Log(Status.Success, _systemTimeProvider.Now));
                await _tableClient.Upsert(data);
            }
            catch (Exception ex)
            {
                await _tableClient.Log(new Log(Status.Failure, _systemTimeProvider.Now));
                _log.LogError(ex.ToString());
            }
            return Unit.Value;
        }
    }
}

