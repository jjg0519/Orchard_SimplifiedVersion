using System;
using System.Collections.Generic;
using OrchardSwagger.Swagger;
using Orchard.Swagger.SwaggerExtensions;

namespace Orchard.Swagger.SwaggerExtensions
{
    public class RecursiveCallSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            schema.properties = new Dictionary<string, Schema>();
            schema.properties.Add("ExtraProperty", schemaRegistry.GetOrRegister(typeof(Product)));
        }
    }
}