using System.Collections.Generic;
using System.Web.Http.Description;
using OrchardSwagger.Swagger;

namespace Orchard.Swagger.SwaggerExtensions
{
    public class AddMessageDefault : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, System.Type type)
        {
            schema.@default = new { title = "A message", content = "Some content" };
        }
    }
}
