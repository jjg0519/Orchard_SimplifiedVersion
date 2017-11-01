using System.Web.Http.Description;

namespace OrchardSwagger.Swagger
{
    public interface IOperationFilter
    {
        void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription);
    }
}
