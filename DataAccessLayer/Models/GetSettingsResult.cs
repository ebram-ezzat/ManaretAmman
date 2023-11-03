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
        public int? StartTimeINMinutes { get; set; }
        public int? EndTimeInMinutes { get; set; }
        public int? AllowedLate { get; set; }
        public int? NoOfDaysBerMonth { get; set; }
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
        public int? breaktime { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public string ReportPath { get; set; }
        public string ReportPathEn { get; set; }
        public string imagepath { get; set; }
        public string footertitle2 { get; set; }
        public string footertitle1 { get; set; }
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
        public int timezone { get; set; }
        public DateTime? validateperiodto { get; set; }
        public int ApprovalProcess { get; set; }
        public int MultiUser { get; set; }
        public int? ISDynamicShift { get; set; }
        public decimal? CompanySharePercent2 { get; set; }
        public decimal? SocialSecurityAmount2 { get; set; }
        public int AllowedMonthlyLeaves { get; set; }
        public int? MissingCheckoutValue { get; set; }
        public int AllowedMonthlyVacation { get; set; }
        public int LeaveWillDeductFromSalary { get; set; }
        public decimal VacationDeductPercentage { get; set; }
        public int IsAllowUserTologin { get; set; }
        public int ExtraDaysWillBeAddedToSalry { get; set; }
        public int AllowMultiCheckIn { get; set; }
        public int BeforeContractPeriod { get; set; }
        public string AttachementPath { get; set; }
        public int ViewSocialSecurity { get; set; }
        public int ZKFlag { get; set; }
        public string SalarySlipReportName { get; set; }
        public int DetailWillBeInShifts { get; set; }
        public decimal SocialSecurityAmount3 { get; set; }
        public decimal? CompanySharePercent3 { get; set; }
        public decimal? LoanPercentage { get; set; }
        public string MainBankID { get; set; }
        public string HrManager { get; set; }
        public decimal IsHourRateInShifts { get; set; }
        public int ByWorkingHours { get; set; }
        public int CompanyMayCLose { get; set; }
        public int ViewEmployeeSalaryeport { get; set; }
        public string WindowsUserPassword { get; set; }
        public string WindowsUserName { get; set; }
        public string WindowsDomain { get; set; }
    }
}
