using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ManaretAmman.MiddleWare
{
    public class AddLanguageHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for the attribute at the controller level
            var controllerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
            var hasLanguageHeaderAttribute = controllerAttributes.OfType<AddLanguageHeaderAttribute>().Any();
            // Optionally, check for the attribute at the action level as well, if needed
             var actionAttributes = context.ApiDescription.CustomAttributes();
             hasLanguageHeaderAttribute = hasLanguageHeaderAttribute || actionAttributes.OfType<AddLanguageHeaderAttribute>().Any();

            if (hasLanguageHeaderAttribute)
            {
                operation.Parameters.Add(new()
                {
                    Name = "LangId",
                    In = ParameterLocation.Header,
                    Description="Default is 1 (Arabic) if you don't send it",
                    Style=ParameterStyle.Form,
                    Required = false // set to false if this is optional
                   
                });
                //if (!operation.Responses.ContainsKey("200"))
                //{
                //    operation.Responses.Add("200", new OpenApiResponse { Description = "OK" });
                //}

                //operation.Responses["200"].Headers ??= new Dictionary<string, OpenApiHeader>();

                //operation.Responses["200"].Headers.Add("LangId", new OpenApiHeader
                //{
                //    Description = "Specifies the language of the response",
                //    Schema = new OpenApiSchema
                //    {
                //        Type = "string"
                //    }
                //});
            }
        
        }

    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AddLanguageHeaderAttribute : Attribute
    {
        // This is a marker attribute and doesn't need to contain any logic
        // Its presence on a controller or action method is enough to signal
        // that the "Language" header should be included in the response
    }
}
