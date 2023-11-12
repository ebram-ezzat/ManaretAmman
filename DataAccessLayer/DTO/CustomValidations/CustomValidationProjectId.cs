using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.CustomValidations
{
    internal class CustomValidationProjectId: ValidationAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomValidationProjectId()
        {
            // Inject the IHttpContextAccessor using constructor injection
            // This assumes you've registered IHttpContextAccessor in the Startup.cs
            _httpContextAccessor = new HttpContextAccessor();
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Access headers or other values from the HttpContext
                var projectId = _httpContextAccessor.HttpContext.Request.Headers["ProjectId"].ToString();


                // Your validation logic using header values
                // For instance:
                if (!string.IsNullOrEmpty(projectId) )
                {
                    // Your business rule passes
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("ProjectId is missing from the request header");
        }
    }
}
