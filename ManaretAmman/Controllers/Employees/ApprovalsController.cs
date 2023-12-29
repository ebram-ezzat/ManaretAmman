using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Approvals;
using BusinessLogicLayer.Services.EmployeeAttendance;
using BusinessLogicLayer.Services.Employees;
using BusinessLogicLayer.Services.Notification;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Notification;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly IApprovalsService _approvalsService;
        private readonly IEmployeeAttendanceService _employeeAttendanceService;

        public ApprovalsController(IApprovalsService approvalsServic, IEmployeeAttendanceService employeeAttendanceService)
        {
            _approvalsService = approvalsServic;
            _employeeAttendanceService = employeeAttendanceService;
        }

        [HttpGet("Vacation/GetPage")]
        public async Task<IApiResponse> VacationGetPage([FromQuery] PaginationFilter<GetEmployeeNotificationInput> filter)
        {
            var result = await _approvalsService.GetVacationApprovalsAsync(filter);

            if (result == null)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<RemiderOutput>>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet("Work/GetPage")]
        public async Task<IApiResponse> WorkGetPage([FromQuery] PaginationFilter<EmployeeAttendanceInput> filter)
        {
            var result = await _employeeAttendanceService.GetEmployeeAttendance(filter);

            if (result == null)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Failure(default, null);
            if (result.Result.Count == 0)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Success("No data", result);


            return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost("Work/SaveWorkEmployeeApprovals")]
        public async Task<IApiResponse> SaveWorkEmployeeApprovals(WorkEmployeeApprovals model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _approvalsService.SaveWorkEmployeeApprovals(model);

            return ApiResponse<int>.Success("data has been retrieved succussfully", result);
        }
        [HttpPost("Work/SaveOverTimeWorkEmployee")]
        public async Task<IApiResponse> SaveOverTimeWorkEmployee(SaveOverTimeWorkEmployee model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _approvalsService.SaveOverTimeWorkEmployee(model);
            if (result == 1)            
                // Success
                return ApiResponse<object>.Success("data has been inserted succussfully",result); // Output parameters
            
           
            return ApiResponse.Failure("Failed to insert overtime work");
            
           
        }
        [HttpDelete("Work/DeleteOverTimeWorkEmployee")]
        public async Task<IApiResponse> DeleteOverTimeWorkEmployee([FromQuery] DeleteOverTimeWorkEmployee model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _approvalsService.DeleteOverTimeWorkEmployee(model);
            if (result == 1)//there is no error, pError=1
                // Success
                return ApiResponse<int>.Success("data has been deleted succussfully", model.EmployeeApprovalID); // Output parameters


            return ApiResponse.Failure("Failed to delete overtime work");


        }
        [HttpGet("Work/GetOverTimeWorkEmployee")]
        public async Task<IApiResponse> GetOverTimeWorkEmployee([FromQuery] GetOverTimeWorkEmployeeInputModel model)
        {
           
            var result = await _approvalsService.GetOverTimeWorkEmployee(model);
            if (result == null)
                return ApiResponse.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);


        }

        [HttpPost("Work/UpdateOverTimeWorkEmployee")]
        public async Task<IApiResponse> UpdateOverTimeWorkEmployee(UpdateOverTimeWorkEmployee model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _approvalsService.UpdateOverTimeWorkEmployee(model);
            if (result == 1)
                // Success
                return ApiResponse<object>.Success("data has been updated succussfully"); // Output parameters


            return ApiResponse.Failure("Failed to update overtime work");


        }
    }
}
