using GSRU_API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;

namespace GSRU_API.Filters
{
    public class RoleAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint == null) return;

            var attributes = endpoint.Metadata.GetOrderedMetadata<HaveRoleAttribute>();
            foreach (var attribute in attributes)
            {
                var rolePrefix = attribute.RolePrefix;
                var paramName = attribute.ParamName;

                var user = context.HttpContext.User;
                var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                object? parameterValue = null;

                if (context.RouteData.Values.TryGetValue(paramName, out var routeValue))
                {
                    parameterValue = routeValue?.ToString() ?? string.Empty;
                }

                if (parameterValue == null && context.HttpContext.Request.Query.TryGetValue(paramName, out var queryValue))
                {
                    parameterValue = queryValue.ToString();
                }

                var bodyValue = await GetBodyParameter(context, paramName);

                if (parameterValue == null && bodyValue != null)
                {
                    parameterValue = bodyValue;
                }

                if (parameterValue == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var expectedRole = $"{rolePrefix}{parameterValue}";

                if (!roles.Contains(expectedRole))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }

        private static async Task<object?> GetBodyParameter(AuthorizationFilterContext context, string paramName)
        {
            object? parameterValue = null;

            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                context.HttpContext.Request.EnableBuffering();
                var body = context.HttpContext.Request.Body;
                using var reader = new StreamReader(body, Encoding.UTF8, leaveOpen: true);
                var bodyContent = await reader.ReadToEndAsync();
                context.HttpContext.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(bodyContent))
                {
                    var jObject = JObject.Parse(bodyContent);
                    var token = jObject.SelectToken(paramName);
                    if (token != null)
                    {
                        parameterValue = token.ToString();
                    }
                }
            }

            return parameterValue;
        }
    }
}