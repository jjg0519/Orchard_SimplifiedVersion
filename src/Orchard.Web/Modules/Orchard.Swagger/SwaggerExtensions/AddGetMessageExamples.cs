using System.Collections.Generic;
using System.Web.Http.Description;
using OrchardSwagger.Swagger;

namespace Orchard.Swagger.SwaggerExtensions
{
    public class AddGetMessageExamples : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var successResponse = operation.responses["200"];
            successResponse.examples = new Dictionary<string, object>()
            {
                { "application/json", new { title = "A message", content = "Some content" } }
            };
        }
    }
}
