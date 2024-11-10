using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.EmployeeAttendance;
using BusinessLogicLayer.Services.Employees;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.Employees;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.EmployeeMonitor
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IEmployeeAttendanceService _employeeAttendanceService;

        public AttendanceController(IEmployeeAttendanceService employeeAttendanceService)
        => _employeeAttendanceService = employeeAttendanceService;

        [HttpGet("GetPage")]
        public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<EmployeeAttendanceInput> filter)
        {
            var result = await _employeeAttendanceService.GetEmployeeAttendance(filter);

            if (result == null)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Failure(default, null);
            if (result.Result.Count == 0)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Success("No data", result);


            return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet("GetEmployeeAttendanceTreatment")]
        public async Task<IApiResponse> GetEmployeeAttendanceTreatment([FromQuery] EmployeeAttendanceTreatmentInput employeeAttendanceTreatmentInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure("An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeAttendanceService.GetEmployeeAttendanceTreatment(employeeAttendanceTreatmentInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
        [HttpPost("SaveEmployeeAttendanceTreatment")]
        public async Task<IApiResponse> SaveEmployeeAttendanceTreatment([FromBody] SaveEmployeeLeaveInput saveEmployeeLeaveInput)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure("An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _employeeAttendanceService.SaveEmployeeAttendanceTreatment(saveEmployeeLeaveInput);

            return ApiResponse<dynamic>.Success("data has been retrieved succussfully", result);
        }
    }
}
