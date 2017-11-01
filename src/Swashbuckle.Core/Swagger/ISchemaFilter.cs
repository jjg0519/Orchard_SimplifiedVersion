using System;

namespace OrchardSwagger.Swagger
{
    public interface ISchemaFilter
    {
        void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type);
    }
}