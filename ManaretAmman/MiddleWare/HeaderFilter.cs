using Azure;
using Microsoft.OpenApi.Models;
using Microsoft.ReportingServices.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ManaretAmman.MiddleWare
{
    public class HeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the controller or action has the [SkipHeaderFilter] attribute
            var hasAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<SkipHeaderFilterAttribute>().Any()
                            || context.MethodInfo.GetCustomAttributes(true).OfType<SkipHeaderFilterAttribute>().Any();

            if (hasAttribute)
            {
                // Skip this filter for the current operation
                return;
            }
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new ()
            {
                Name = "ProjectId",
                In = ParameterLocation.Header,               
                Required = true // set to false if this is optional
            });
            operation.Parameters.Add(new()
            {
                Name = "UserId",
                In = ParameterLocation.Header,
                Required = false // set to false if this is optional
            });
        }
        private void AddHeaderIfNotExists(IList<OpenApiParameter> parameters, string headerName, ParameterLocation location, bool required)
        {
            if (!parameters.Any(p => p.Name.Equals(headerName, StringComparison.OrdinalIgnoreCase)))
            {
                parameters.Add(new OpenApiParameter
                {
                    Name = headerName,
                    In = location,
                    Required = required
                });
            }
        }

    }
    #region skip project id attrubute from required swaager header
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SkipHeaderFilterAttribute : Attribute
    {
    }
    #endregion
}
