﻿using BusinessLogicLayer.Common;
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
        [HttpGet("GetEmployeeSalaryReportV2")]
        public async Task<IApiResponse> GetEmployeeSalaryReportV2([FromQuery] GetEmployeeSalaryReportRequestV2 getEmployeeSalaryReport)
        {
            var result = await _reportService.GetEmployeeSalaryReportV2(getEmployeeSalaryReport);

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
        #region تقرير الرواتب 
        /// <summary>
        /// تقرير الرواتب 
        /// 
        /// </summary>  
        /// <remarks>
        /// <para>you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,</para>
        /// <para>you should send {UserId} Via header,</para>
        /// <para>{Flag} here is 1</para>
        /// <para>{IsAllEmployees} here is 0</para> 
        /// <para>{WithIbanOnly} here is null</para> 
        /// </remarks>
        /// <param name="getEmployeeSaleriesReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeSaleriesReport")]
        public async Task<IApiResponse> GetEmployeeSaleriesReport([FromQuery] GetEmployeeSaleriesReportRequest getEmployeeSaleriesReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeSaleriesReport(getEmployeeSaleriesReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion
        #region تقرير التحويل البنكى
        /// <summary>
        /// تقرير التحويل البنكى 
        /// 
        /// </summary>  
        /// <remarks>
        /// <para>you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,</para>
        /// <para>you should send {UserId} Via header,</para>
        /// <para>{Flag} here is 1</para>
        /// <para>{IsAllEmployees} here is 1</para> 
        /// <para>{WithIbanOnly} here is 1</para> 
        /// </remarks>
        /// <param name="getEmployeeBankConvertReportRequest"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeBankConvertReport")]
        public async Task<IApiResponse> GetEmployeeBankConvertReport([FromQuery] GetEmployeeBankConvertReportRequest getEmployeeBankConvertReportRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeeBankConvertReport(getEmployeeBankConvertReportRequest);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region تقرير عقوبات الموظفين
        /// <summary>
        ///  تقرير عقوبات الموظفين
        /// 
        /// </summary>  
        /// <remarks>
        /// <para>you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,</para>
        /// <para>you should send {UserId} Via header,</para>
        /// </remarks>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeePenaltyReport")]
        public async Task<IApiResponse> GetEmployeePenaltyReport([FromQuery] GetEmployeePenaltyReport getEmployeePenaltyReport)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmployeePenaltyReport(getEmployeePenaltyReport);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }
        #endregion

        #region  تقرير الرواتب وتقرير العلاوات والاقتطاعات

        /// <summary>
        /// تقرير الرواتب وتقرير العلاوات والاقتطاعات 
        /// 
        /// </summary>  
        /// <remarks>
        /// <para>you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,</para>
        /// <para>you should send {UserId} Via header,</para>
        /// <para>{Flag} here is 3 with Allowances and deductions and 1 with Employee Salary</para>
        /// <para> {IsAllEmployees} is 1 in all employees salary, otherwise is 0</para>
        /// </remarks>
        /// <param name="getEmployeeSalaryReport"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetEmployeeMonthlySalaryReport")]
        public async Task<IApiResponse> GetEmployeeSalaryReport([FromQuery] GetEmployeeSalaryReport getEmployeeSalaryReport)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetEmpSalaryReport(getEmployeeSalaryReport);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }

        #endregion

        #region تقرير العلاوات وتقرير الاقتطاعات 

        /// <summary>
        ///  تقرير العلاوات وتقرير الاقتطاعات 
        /// 
        /// </summary>  
        /// <remarks>
        /// <para>you should send {Accept-Language} Via header request to get the correct description  "ar" For Arabic and "en" For English,</para>
        /// <para>you should send {UserId} Via header,</para>
        /// <para>{TypeID} here is 3 with deductions and 2 with Allowances</para>
        /// </remarks>
        /// <param name="getAllowancesDeductionsReport"></param>
        /// <returns></returns>
        [AddLanguageHeader]
        [HttpGet("GetAllowancesDeductionsReport")]
        public async Task<IApiResponse> GetAllowancesDeductionsReport([FromQuery] GetEmployeeSalaryReport getAllowancesDeductionsReport)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed based on data annotations including your custom validation
                // Retrieve error messages
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return ApiResponse.Failure(" An unexpected error on validation occurred", errors.ToArray());
            }
            var result = await _reportService.GetAllowancesDeductionsReport(getAllowancesDeductionsReport);

            if (result == null)
                return ApiResponse<object>.Failure("No data", null);


            return ApiResponse<object>.Success("data has been retrieved succussfully", result);
        }

        #endregion
    }
}
