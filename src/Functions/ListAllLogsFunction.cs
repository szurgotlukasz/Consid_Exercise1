using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Exercise1.Queries.ListAllLogs;
using MediatR;

namespace Exercise1.Functions
{
    public class ListAllLogsFunction
    {
        private readonly IMediator mediator;

        public ListAllLogsFunction(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [FunctionName("ListAllLogsFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Function {nameof(ListAllLogsFunction)} has started");
            string fromParam = req.Query["from"];
            string toParam = req.Query["to"];

            bool fromIsValid = DateTimeOffset.TryParse(fromParam, out var from);
            bool toIsValid = DateTimeOffset.TryParse(toParam, out var to);
            bool fromBeforeTo = DateTimeOffset.Compare(from, to) > 0;

            if (!fromIsValid || !toIsValid || fromBeforeTo)
            {
                log.LogInformation($"Specified dates from/to are invalid. Dates: from - {fromParam} ; to - {toParam}");
                return new BadRequestObjectResult("Please provide valid 'from' and 'to' date parameters in the query string.");
            }

            var query = new ListAllLogsQuery(from, to);

            var response = await mediator.Send(query);
            return new OkObjectResult(response);
        }
    }
}

