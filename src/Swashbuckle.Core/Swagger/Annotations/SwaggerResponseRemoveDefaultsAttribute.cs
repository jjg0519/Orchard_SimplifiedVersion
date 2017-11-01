using System;
using System.Net;

namespace OrchardSwagger.Swagger.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerResponseRemoveDefaultsAttribute : Attribute
    {
    }
}