using Exercise1.Commands.FetchData;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FetchDataTimerFunction
{
    public class FetchDataTimerFunction
    {
        private readonly IMediator _mediator;

        public FetchDataTimerFunction(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [FunctionName("FetchDataTimerFunction")]
        public void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        {
            _mediator.Send(new FetchDataCommand());
        }
    }
}

