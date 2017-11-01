using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using Orchard.WebApi.Extensions;
using System.Diagnostics.Contracts;
using System.Web.Http.Tracing;
using System.Collections.Concurrent;
using System.Linq;

namespace Orchard.WebApi {
    public class DefaultOrchardWebApiHttpControllerSelector :DefaultHttpControllerSelector,IHttpControllerSelector {
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;
        public DefaultOrchardWebApiHttpControllerSelector(HttpConfiguration configuration) : base(configuration) {
            _controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(InitializeControllerInfoCache);
            _configuration = configuration;
        }

        /// <summary>
        /// Tries to resolve an instance for the controller associated with a given service key for the work context scope.
        /// </summary>
        /// <typeparam name="T">The type of the controller.</typeparam>
        /// <param name="workContext">The work context.</param>
        /// <param name="serviceKey">The service key for the controller.</param>
        /// <param name="instance">The controller instance.</param>
        /// <returns>True if the controller was resolved; false otherwise.</returns>
        protected bool TryResolve<T>(WorkContext workContext, object serviceKey, out T instance) {
            if (workContext != null && serviceKey != null) {
                var key = new KeyedService(serviceKey, typeof(T));
                object value;
                if (workContext.Resolve<ILifetimeScope>().TryResolveService(key, out value)) {
                    instance = (T)value;
                    return true;
                }
            }

            instance = default(T);
            return false;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();

            // Determine the area name for the request, and fall back to stock orchard controllers
            var areaName = routeData.GetAreaName();

            var controllerName = base.GetControllerName(request);

            // Service name pattern matches the identification strategy
            var serviceKey = (areaName + "/" + controllerName).ToLowerInvariant();

            var controllerContext = new HttpControllerContext(_configuration, routeData, request);

            // Now that the request container is known - try to resolve the controller information
            Meta<Lazy<IHttpController>> info;
            var workContext = controllerContext.GetWorkContext();
            if (TryResolve(workContext, serviceKey, out info))
            {
                var type = (Type)info.Metadata["ControllerType"];

                return
                    new HttpControllerDescriptor(_configuration, controllerName, type);
            }

            return null;
        }
        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return _controllerInfoCache.Value.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }
        private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            var result = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            var duplicateControllers = new HashSet<string>();
            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
            var groupedByName = controllerTypes.GroupBy(
                t => t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length),
                StringComparer.OrdinalIgnoreCase);

            var _controllerTypeCacheCache = groupedByName.ToDictionary(
                g => g.Key,
                g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                StringComparer.OrdinalIgnoreCase);
            Dictionary<string, ILookup<string, Type>> controllerTypeGroups = _controllerTypeCacheCache;

            foreach (KeyValuePair<string, ILookup<string, Type>> controllerTypeGroup in controllerTypeGroups)
            {
                string controllerName = controllerTypeGroup.Key;

                foreach (IGrouping<string, Type> controllerTypesGroupedByNs in controllerTypeGroup.Value)
                {
                    foreach (Type controllerType in controllerTypesGroupedByNs)
                    {
                        if (result.Keys.Contains(controllerName))
                        {
                            duplicateControllers.Add(controllerName);
                            break;
                        }
                        else
                        {
                            result.TryAdd(controllerName, new HttpControllerDescriptor(_configuration, controllerName, controllerType));
                        }
                    }
                }
            }

            foreach (string duplicateController in duplicateControllers)
            {
                HttpControllerDescriptor descriptor;
                result.TryRemove(duplicateController, out descriptor);
            }

            return result;
        }
    }
}