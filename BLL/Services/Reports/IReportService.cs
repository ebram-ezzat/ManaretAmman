﻿using DataAccessLayer.DTO.Employees;
using DataAccessLayer.DTO.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Reports
{
    public interface IReportService
    {
        Task<object> GetEmployeeSalaryReport(GetEmployeeSalaryReportRequest getEmployeePaperRequest);
        Task<object> GetEmployeeAttendanceDailyReport(GetEmployeeAttendanceDailyRequest getEmployeeAttendanceDailyRequest);
        Task<object> GetEmployeeAttendanceDailyDetailedReport(GetEmployeeAttendanceDailyDetailedReportRequest getEmployeeAttendanceDailyDetailedReportRequest);
        Task<object> GetEmployeeOverTimeWorkReport(GetEmployeeOverTimeWorkReportRequest getEmployeeOverTimeReportRequest);
        Task<object> GetEmployeeMorningLateReport(GetEmployeeMorningLateReportRequest getEmployeeMorningLateReportRequest);
        Task<object> GetEmployeeEarlyLeaveReport(GetEmployeeEarlyLeaveReportRequest getEmployeeEarlyLeaveReportRequest);
        Task<object> GetEmployeeAbsentsReport(GetEmployeeAbsentsReportRequest getEmployeeAbsentsReportRequest);
        Task<object> GetEmployeeAffairsServiceReport(GetEmployeeAffairsServiceReportRequest getEmployeeAffairsServiceReportRequest);
        Task<object> GetEmployeeSaleriesReport(GetEmployeeSaleriesReportRequest getEmployeeSaleriesReportRequest);
        Task<object> GetEmployeeBankConvertReport(GetEmployeeBankConvertReportRequest getEmployeeBankConvertReportRequest);
        Task<object> GetEmployeePenaltyReport(GetEmployeePenaltyReport getEmployeePenaltyReport);
        Task<object> GetEmpSalaryReport(GetEmployeeSalaryReport getEmployeeSalaryReport);
        Task<object> GetAllowancesDeductionsReport(GetEmployeeSalaryReport getEmployeeSalaryReport);
        Task<object> GetEmployeeSalaryReportV2(GetEmployeeSalaryReportRequestV2 getEmployeeSalaryReportRequest);

    }
}
