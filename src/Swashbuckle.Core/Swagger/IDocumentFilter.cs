using System.Web.Http.Description;

namespace OrchardSwagger.Swagger
{
    public interface IDocumentFilter
    {
        void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer);
    }
}
