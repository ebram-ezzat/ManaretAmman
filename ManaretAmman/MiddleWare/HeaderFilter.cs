using Microsoft.OpenApi.Models;
using Microsoft.ReportingServices.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ManaretAmman.MiddleWare
{
    public class HeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
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
    }
}
