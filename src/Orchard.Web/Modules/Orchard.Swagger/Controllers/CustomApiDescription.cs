using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Orchard.Swagger.Controllers
{
    /// <summary> Describes an API defined by relative URI path and HTTP method. </summary>
	public class CustomApiDescription:ApiDescription
    {
        public CustomApiDescription()
        {
            this.SupportedRequestBodyFormatters = new System.Collections.ObjectModel.Collection<System.Net.Http.Formatting.MediaTypeFormatter>();
            this.SupportedResponseFormatters = new System.Collections.ObjectModel.Collection<System.Net.Http.Formatting.MediaTypeFormatter>();
            this.ParameterDescriptions = new System.Collections.ObjectModel.Collection<System.Web.Http.Description.ApiParameterDescription>();
        }

        /// <summary> Gets or sets the HTTP method. </summary>
        /// <returns> The HTTP method. </returns>
        public System.Net.Http.HttpMethod HttpMethod
        {
            get;
            set;
        }

        /// <summary> Gets or sets the relative path. </summary>
        /// <returns> The relative path. </returns>
        public  string RelativePath
        {
            get;
            set;
        }

        /// <summary> Gets or sets the action descriptor that will handle the API. </summary>
        /// <returns> The action descriptor. </returns>
        public  System.Web.Http.Controllers.HttpActionDescriptor ActionDescriptor
        {
            get;
            set;
        }

        /// <summary> Gets or sets the registered route for the API. </summary>
        /// <returns> The route. </returns>
        public  System.Web.Http.Routing.IHttpRoute Route
        {
            get;
            set;
        }

        /// <summary> Gets or sets the documentation of the API. </summary>
        /// <returns> The documentation. </returns>
        public  string Documentation
        {
            get;
            set;
        }

        /// <summary> Gets the supported response formatters. </summary>
        public  System.Collections.ObjectModel.Collection<System.Net.Http.Formatting.MediaTypeFormatter> SupportedResponseFormatters
        {
            get;
            internal set;
        }

        /// <summary> Gets the supported request body formatters. </summary>
        public  System.Collections.ObjectModel.Collection<System.Net.Http.Formatting.MediaTypeFormatter> SupportedRequestBodyFormatters
        {
            get;
            internal set;
        }

        /// <summary> Gets the parameter descriptions. </summary>
        public  System.Collections.ObjectModel.Collection<System.Web.Http.Description.ApiParameterDescription> ParameterDescriptions
        {
            get;
            internal set;
        }

        /// <summary> Gets the ID. The ID is unique within <see cref="T:System.Web.Http.HttpServer" />. </summary>
        public  string ID
        {
            get
            {
                return ((this.HttpMethod != null) ? this.HttpMethod.Method : string.Empty) + (this.RelativePath ?? string.Empty);
            }
        }

        /// <summary> Initializes a new instance of the <see cref="T:System.Web.Http.Description.ApiDescription" /> class. </summary>
    }
}