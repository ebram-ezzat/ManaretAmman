using BusinessLogicLayer.Services.Employees;
using DataAccessLayer.DTO.Employees;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetList")]
        public async Task<IApiResponse> GetList() 
        {
            var result = await _employeeService.GetEmployeesProc();

            return ApiResponse<List<EmployeeLookup>>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("SaveAttendanceByUser")]
        public async Task<IApiResponse> SaveAttendanceByUser(SaveAttendance saveAttendance)
        {
            await _employeeService.SaveAttendanceByUser(saveAttendance);

            return ApiResponse.Success("data has been retrieved succussfully");
        }
        [HttpGet("Documents/GetEmployeePaper")]
        public async Task<IApiResponse> GetEmployeePaper([FromQuery] GetEmployeePaperRequest getEmployeePaperRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeService.GetEmployeePaperProc(getEmployeePaperRequest);



            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        [HttpGet("Documents/DeleteEmployeePaper")]
        public async Task<IApiResponse> DeleteEmployeePaper(int EmployeeId,int DetailId)
        {
            if (EmployeeId<=0 || DetailId<=0)
            {

                return ApiResponse.Failure(" An unexpected error on validation occurred you should path each parameter bigger than 0",null);
            }
            var result = await _employeeService.DeleteEmployeePaperProc(EmployeeId, DetailId);



            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }
        public async Task<IApiResponse> SaveEmployeePaper(SaveEmployeePaper saveEmployeePaper)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
           // var result = await _employeeService.DeleteEmployeePaperProc(EmployeeId, DetailId);



            return ApiResponse<int>.Success("data has been retrieved succussfully", 0);
        }

    }
}
