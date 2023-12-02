using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Approvals;
using BusinessLogicLayer.Services.EmployeeAttendance;
using BusinessLogicLayer.Services.Reports;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.DTO.Reports;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("Reports/GetEmployeeSalaryReport")]
        public async Task<IApiResponse> GetEmployeeSalaryReport([FromQuery] GetEmployeeSalaryReportRequest getEmployeeSalaryReport)
        {
            var result = await _reportService.GetEmployeeSalaryReport(getEmployeeSalaryReport);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }

    }
}
