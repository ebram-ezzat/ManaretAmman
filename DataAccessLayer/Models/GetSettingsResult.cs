﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetSettingsResult
    {
       
        public int SettingID { get; set; }
        public decimal? AdditionalInWork { get; set; }
        public decimal? AdditionalInHoliday { get; set; }
        public int? CalculateAdditionalIfExceed { get; set; }
        public decimal? SocialSecurityAllowanceID { get; set; }
        public int? CalLeavesFromSalary { get; set; }
        public int? CalLateAsAdditional { get; set; }
        public int? DailyWorkInMinutes { get; set; }
        public int? StartTimeInMinutes { get; set; }
        public int? EndTimeInMinutes { get; set; }
        public int? AllowedLate { get; set; }
        public int NoOfDaysBerMonth { get; set; }
        public int? WithMorningExtra { get; set; }
        public int? CheckoutAfterMidNight { get; set; }
        public int? AllowedLateInLeave { get; set; }
        public int? ActiveYear { get; set; }
        public int? DependOnCheckType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? AdditionalAllowanceID { get; set; }
        public int? PersonalVacationID { get; set; }
        public string MonthID { get; set; }
        public int? LastCalculationDate { get; set; }
        public int? GlobalLocal { get; set; }
        public int? Test { get; set; }
        public int? WithBreak { get; set; }
        public int? BreakTime { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public string ReportPath { get; set; }
        public string ReportPathEn { get; set; }
        public string ImagePath { get; set; }
        public string FooterTitle2 { get; set; }
        public string FooterTitle1 { get; set; }
        public int? IsActive { get; set; }
        public int? LateDeductType { get; set; }
        public decimal? LateDeductPercentage { get; set; }
        public decimal? CompanySharePercent { get; set; }
        public int? BreakFrom { get; set; }
        public int? BreakTo { get; set; }
        public string CurrentCulture { get; set; }
        public string CurrentUICulture { get; set; }
        public string DateTimeFormat { get; set; }
        public string DateSeparator { get; set; }
        public string ShortTimePattern { get; set; }
        public string ShortDatePattern { get; set; }
        public string DateFormatForGrid { get; set; }
        public int? EarlyLeaveDeductType { get; set; }
        public decimal? EarlyLeaveDeductPercentage { get; set; }
        public int? Timezone { get; set; }
        public DateTime? ValidatePeriodTo { get; set; }
        public int? ApprovalProcess { get; set; }
        public int? MultiUser { get; set; }
        public int? IsDynamicShift { get; set; }
        public decimal? CompanySharePercent2 { get; set; }
        public decimal? SocialSecurityAmount2 { get; set; }
        public int? AllowedMonthlyLeaves { get; set; }
        public int? MissingCheckoutValue { get; set; }
        public int? AllowedMonthlyVacation { get; set; }
        public int? LeaveWillDeductFromSalary { get; set; }
        public decimal? VacationDeductPercentage { get; set; }
        public int? IsAllowUserToLogin { get; set; }
        public int? ExtraDaysWillBeAddedToSalry { get; set; }
        public int? AllowMultiCheckIn { get; set; }
        public int? BeforeContractPeriod { get; set; }
        public int? ViewSocialSecurity { get; set; }
        public int? ZKFlag { get; set; }
        public string SalarySlipReportName { get; set; }
        public int? DetailWillBeInShifts { get; set; }
        public decimal? SocialSecurityAmount3 { get; set; }
        public decimal? CompanySharePercent3 { get; set; }
        public decimal? LoanPercentage { get; set; }
        public int? MainBankID { get; set; }
        public string HrManager { get; set; }
        public decimal? IsHourRateInShifts { get; set; }
        public int? ByWorkingHours { get; set; }
        public int? CompanyMayClose { get; set; }
        public int? ViewEmployeeSalaryeport { get; set; }
        public string WindowsUserPassword { get; set; }
        public string WindowsUserName { get; set; }
        public string WindowsDomain { get; set; }
        public int? CompanyHasUnderTestStatus { get; set; }
        public int? SettingPage { get; set; }
        public int? IsBasic { get; set; }
        public string CheckinKey { get; set; }
        public string CheckoutKey { get; set; }
        public string BreakinKey { get; set; }
        public string BreakoutKey { get; set; }
        public decimal? CountLateWillDeductAsInDays { get; set; }
        public decimal? CountEarlyLeaveWillDeductAsInDays { get; set; }
        public int? IsWithApproval { get; set; }
        public int? SSNINSalarySlip { get; set; }
        public decimal? BreakLadeDeductPercentage { get; set; }
        public int? BreakWillDeductFromLeaveInMinutes { get; set; }
        public string SalaryReportAggregateName { get; set; }
        public string LoanReportName { get; set; }
        public string ReportBankConvertName { get; set; }
        public int? ShowCompanyNameID { get; set; }
        public int? EmailPort { get; set; }
        public string EmailHost { get; set; }
        public string EmailPassword { get; set; }
        public string EmailFrom { get; set; }
        public int? isviewAssuranceTemplte { get; set; }
        public int? ShowEvaluationPage { get; set; }
        public decimal? MissingBreakInValue { get; set; }
        public decimal? MissingBreakOutValue { get; set; }
        public string DailyAttendanceReportName { get; set; }
        public string DailyAttendanceReportDetails { get; set; }
        public string EmployeeReportDetailSalary { get; set; }
        public string EmployeeReportDetailSalaryAll { get; set; }
        public int? RamadanEndDate { get; set; }
        public int? RamadanStartDate { get; set; }
        public int? IsCompletedRamadanSetting { get; set; }
        public int? EnableSecondPeriod { get; set; }
        public string AttachementPath { get; set; }
        public int? IsviewTimeDr { get; set; }
        public string DailyAttendanceByEmployeeReportName { get; set; }
        public string DailyAttendanceByAllEmployeeReportName { get; set; }
        public string DailyAbsentByAllEmployeeReportName { get; set; }
        public string DailyAbsentByEmployeeReportName { get; set; }
        public string DailyEarlyLeaveByAllEmployeeReportName { get; set; }
        public string DailyEarlyLeaveByEmployeeReportName { get; set; }
        public string DailyLateByAllEmployeeReportName { get; set; }
        public string DailyLateByEmployeeReportName { get; set; }
        public string DailyAdditionalWorkByAllEmployeeReportName { get; set; }
        public string DailyAdditionalWorkByEmployeeReportName { get; set; }       
        public string DailyAbsentEmployeesReportName { get; set; }
        public string DailyEarlyLeaveReportName { get; set; }
        public string DailyLateReportName { get; set; }
        public string DailyAdditionalWorkReportName { get; set; }
        public string CompanyNationalID { get; set; }
        public string EmployeeDeduction { get; set; }
        public string EmployeeAllowance { get; set; }
        public string EmployeeSalaryDetails { get; set; }
        // public string HRServiceReportName { get; set; }
    }
}
