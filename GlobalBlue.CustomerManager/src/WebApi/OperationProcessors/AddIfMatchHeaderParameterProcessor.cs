using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace GlobalBlue.CustomerManager.WebApi.OperationProcessors
{
    public class AddIfMatchHeaderParameterProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(new NSwag.OpenApiParameter
            {
                Kind = NSwag.OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = true,
                Name = "If-Match",
                Description = "The valid ETag for the corresponding entity to be updated. Use GET api/customers/{id} endpoint to fetch the ETag from the response header for a paritcular resource.",
            });

            return true;
        }
    }
}
