using System.Web.Http.Description;
using OrchardSwagger.Swagger;

namespace Orchard.Swagger.SwaggerExtensions
{
    public class UpdateFileDownloadOperations : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId == "FileDownload_GetFile")
            {
                operation.produces = new[] { "application/octet-stream" };
            }
        }
    }
}