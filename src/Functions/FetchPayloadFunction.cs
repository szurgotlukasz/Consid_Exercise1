using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using Exercise1.Queries;

namespace Exercise1.Functions
{
    public class FetchPayloadFunction
    {
        private readonly IMediator mediator;

        public FetchPayloadFunction(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [FunctionName("FetchPayloadFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Function {nameof(FetchPayloadFunction)} has started");
            string idParam = req.Query["id"];
            if(!Guid.TryParse(idParam, out var id))
            {
                log.LogInformation($"Specified guid id was invalid. parameter: {idParam}");
                return new BadRequestObjectResult("Please provide valid 'id' guid parameter in the query string.");
            }

            var query = new FetchPayloadQuery(id);
            var response = await mediator.Send(query);
            if (response.Exists)
                return new OkObjectResult(response.Payload);
            else
                return new NotFoundResult();
        }
    }
}

