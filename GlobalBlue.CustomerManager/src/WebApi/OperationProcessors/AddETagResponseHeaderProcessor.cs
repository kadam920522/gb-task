using Microsoft.AspNetCore.Http;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace GlobalBlue.CustomerManager.WebApi.OperationProcessors
{
    public class AddETagResponseHeaderProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var response = context.OperationDescription.Operation.Responses[StatusCodes.Status200OK.ToString()];
            response.Headers["ETag"] = new NSwag.OpenApiHeader() { Description = "Entity tags uniquely representing the requested resources. Use it for concurrency handling." };

            return true;
        }
    }
}
