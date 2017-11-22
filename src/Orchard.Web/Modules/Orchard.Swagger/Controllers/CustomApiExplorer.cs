using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using System.Web.Http.ValueProviders;

namespace Orchard.Swagger
{

    /// <summary> Explores the URI space of the service based on routes, controllers and actions available in the system. </summary>
    public class CustomApiExplorer:ApiExplorer
    {
        public CustomApiExplorer(HttpConfiguration configuration) : base(configuration)
        {
        }
        /// <summary>
        /// 确定是否应考虑将此操作用于生成 System.Web.Http.Description.ApiExplorer.ApiDescriptions。初始化 System.Web.Http.Description.ApiExplorer.ApiDescriptions时调用。
        /// </summary>
        /// <param name="actionVariableValue">来自路由的操作变量值</param>
        /// <param name="actionDescriptor">操作描述符.</param>
        /// <param name="route">路由</param>
        /// <returns>如果应考虑将此操作用于生成 System.Web.Http.Description.ApiExplorer.ApiDescriptions，则为 true，否则为 false。</returns>
        public override bool ShouldExploreAction(string actionVariableValue, HttpActionDescriptor actionDescriptor, IHttpRoute route)
        {
            if (actionDescriptor == null)
            {
                throw new ArgumentNullException("actionDescriptor");
            }

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            ApiExplorerSettingsAttribute setting = actionDescriptor.GetCustomAttributes<ApiExplorerSettingsAttribute>().FirstOrDefault();
            NonActionAttribute nonAction = actionDescriptor.GetCustomAttributes<NonActionAttribute>().FirstOrDefault();
            string hcNamespace = string.Empty;
            string routetemplate = string.Empty;
            routetemplate = route.RouteTemplate.Split('/')[1];
            hcNamespace = actionDescriptor.ControllerDescriptor.ControllerType.Namespace;
            if (hcNamespace.Contains(routetemplate))
            {
                return (setting == null || !setting.IgnoreApi) &&
                  (nonAction == null) &&
                  MatchRegexConstraint(route, "action", actionVariableValue);
            }
            else
            {
                return false;
            }
        }
        public override bool ShouldExploreController(string controllerVariableValue, HttpControllerDescriptor controllerDescriptor, IHttpRoute route)
        {
            if (controllerDescriptor == null)
            {
                throw new ArgumentNullException("controllerDescriptor");
            }

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            ApiExplorerSettingsAttribute setting = controllerDescriptor.GetCustomAttributes<ApiExplorerSettingsAttribute>().FirstOrDefault();
            string hcNamespace = string.Empty;
            string routetemplate = string.Empty;
            routetemplate = route.RouteTemplate.Split('/')[1];
            hcNamespace = controllerDescriptor.ControllerType.Namespace;
            if (hcNamespace.Contains(routetemplate))
            {
                return (setting == null || !setting.IgnoreApi) &&
MatchRegexConstraint(route, "controller", controllerVariableValue);
            }
            else
            {
                return false;
            }

        }
        private static bool MatchRegexConstraint(IHttpRoute route, string parameterName, string parameterValue)
        {
            IDictionary<string, object> constraints = route.Constraints;
            if (constraints != null)
            {
                object constraint;
                if (constraints.TryGetValue(parameterName, out constraint))
                {
                    // treat the constraint as a string which represents a Regex.
                    // note that we don't support custom constraint (IHttpRouteConstraint) because it might rely on the request and some runtime states
                    string constraintsRule = constraint as string;
                    if (constraintsRule != null)
                    {
                        string constraintsRegEx = "^(" + constraintsRule + ")$";
                        return parameterValue != null && Regex.IsMatch(parameterValue, constraintsRegEx, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    }
                }
            }
            return true;
        }

    }
}
