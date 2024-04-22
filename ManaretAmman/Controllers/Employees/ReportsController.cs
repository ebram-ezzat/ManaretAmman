using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.Approvals;
using BusinessLogicLayer.Services.EmployeeAttendance;
using BusinessLogicLayer.Services.Reports;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.DTO.Reports;
using ManaretAmman.MiddleWare;
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
        [HttpGet("GetEmployeeSalaryReport")]
        public async Task<IApiResponse> GetEmployeeSalaryReport([FromQuery] GetEmployeeSalaryReportRequest getEmployeeSalaryReport)
        {
            var result = await _reportService.GetEmployeeSalaryReport(getEmployeeSalaryReport);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }

        #region تقرير شاشة خدمات شوون الموظفين 
        /// <summary>
        ///   "تقرير شاشة خدمات شوون الموظفين "الطباعة 
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,       
        /// </remarks>
        /// <param name="getEmployeeAffairsServiceReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeAffairsServiceReport")]
        public async Task<IApiResponse> GetEmployeeAffairsServiceReport([FromQuery] GetEmployeeAffairsServiceReportRequest getEmployeeAffairsServiceReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeAffairsServiceReport(getEmployeeAffairsServiceReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region التقرير اليومى
        /// <summary>
        /// التقرير اليومى
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,
        /// {Flag} here is 1
        /// </remarks>
        /// <param name="getEmployeeAttendanceDailyRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeAttendanceDailyReport")]
        public async Task<IApiResponse> GetEmployeeAttendanceDailyReport([FromQuery] GetEmployeeAttendanceDailyRequest getEmployeeAttendanceDailyRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeAttendanceDailyReport(getEmployeeAttendanceDailyRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region    التقرير اليومى التفصيلى
        /// <summary>
        ///التقرير اليومى التفصيلى
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,
        /// {Flag} here is 1,
        /// {ReportType} here is always 0 that is meaning Date
        /// </remarks>
        /// <param name="getEmployeeAttendanceDailyDetailedReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeAttendanceDailyDetailedReport")]
        public async Task<IApiResponse> GetEmployeeAttendanceDailyDetailedReport([FromQuery] GetEmployeeAttendanceDailyDetailedReportRequest getEmployeeAttendanceDailyDetailedReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeAttendanceDailyDetailedReport(getEmployeeAttendanceDailyDetailedReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region   تقرير العمل الضافي
        /// <summary>
        ///تقرير العمل الضافي
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,
        /// {Flag} here is 1
        /// </remarks>
        /// <param name="getEmployeeOverTimeWorkReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeOverTimeWorkReport")]
        public async Task<IApiResponse> GetEmployeeOverTimeWorkReport([FromQuery] GetEmployeeOverTimeWorkReportRequest getEmployeeOverTimeWorkReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeOverTimeWorkReport(getEmployeeOverTimeWorkReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region   تقرير التاخير الصباحى
        /// <summary>
        ///تقرير التاخير الصباحى
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,
        /// {Flag} here is 1
        /// </remarks>
        /// <param name="getEmployeeMorningLateReportReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeMorningLateReport")]
        public async Task<IApiResponse> GetEmployeeMorningLateReport([FromQuery] GetEmployeeMorningLateReportRequest getEmployeeMorningLateReportReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeMorningLateReport(getEmployeeMorningLateReportReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion
        #region   تقرير الخروج المبكر
        /// <summary>
        ///تقرير الخروج المبكر
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,
        /// {Flag} here is 2
        /// </remarks>
        /// <param name="getEmployeeEarlyLeaveReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeEarlyLeaveReport")]
        public async Task<IApiResponse> GetEmployeeEarlyLeaveReport([FromQuery] GetEmployeeEarlyLeaveReportRequest getEmployeeEarlyLeaveReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeEarlyLeaveReport(getEmployeeEarlyLeaveReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion
        #region  تقرير الغيابات 
        /// <summary>
        ///تقرير الغيابات
        /// 
        /// </summary>  
        /// <remarks>
        /// you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,
        /// you should send {UserId} Via header,
        /// {Flag} here is 2
        /// </remarks>
        /// <param name="getEmployeeAbsentsReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeAbsentsReport")]
        public async Task<IApiResponse> GetEmployeeAbsentsReport([FromQuery] GetEmployeeAbsentsReportRequest getEmployeeAbsentsReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeAbsentsReport(getEmployeeAbsentsReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion
    }
}
