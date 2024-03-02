using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace ManaretAmman.MiddleWare
{
    public class IncludeModelPropertyDescriptionsFilter : IOperationFilter
    {
        private readonly XDocument _xmlDoc;

        public IncludeModelPropertyDescriptionsFilter(string xmlDocPath)
        {
            _xmlDoc = XDocument.Load(xmlDocPath);
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var parameter in operation.Parameters)
            {
                var description = GetPropertyDescription(parameter.Name, context.MethodInfo.DeclaringType);
                if (!string.IsNullOrEmpty(description))
                {
                    parameter.Description += " " + description;
                }
            }
        }

        private string GetPropertyDescription(string propertyName, Type type)
        {
            // Assumes XML documentation tags are correctly formatted and exist.
            var memberName = $"P:{type.FullName}.{propertyName}";
            var member = _xmlDoc.Descendants("member")
                                .FirstOrDefault(m => m.Attribute("name")?.Value == memberName);
            var summary = member?.Element("summary")?.Value.Trim();
            return summary;
        }
    }
}
