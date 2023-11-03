﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Index("ProjectID", Name = "IX_Setting_P")]
    public partial class Setting
    {
        [Key]
        public int SettingID { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? AdditionalInWork { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? AdditionalInHoliday { get; set; }
        public int? CalculateAdditionalIfExceed { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
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
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        public int? AdditionalAllowanceID { get; set; }
        public int? PersonalVacationID { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string MonthID { get; set; }
        public int? LastCalculationDate { get; set; }
        public int? GlobalLocal { get; set; }
        public int? Test { get; set; }
        public int? WithBreak { get; set; }
        public int? breaktime { get; set; }
        [Key]
        public int ProjectID { get; set; }
        public int? LateDeductType { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? LateDeductPercentage { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CompanySharePercent { get; set; }
        public int? BreakFrom { get; set; }
        public int? BreakTo { get; set; }
        public int? EarlyLeaveDeductType { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? EarlyLeaveDeductPercentage { get; set; }
        public int? timezone { get; set; }
        public int? ApprovalProcess { get; set; }
        public int? MultiUser { get; set; }
        public int? ISDynamicShift { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CompanySharePercent2 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SocialSecurityAmount2 { get; set; }
        public int? WithIncomeTax { get; set; }
        public int? AllowedMonthlyLeaves { get; set; }
        public int? MissingCheckoutValue { get; set; }
        public int? DoubleHolidayinThursday { get; set; }
        public int? LeaveWillDeductFromSalary { get; set; }
        public int? AllowedMonthlyVacation { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? VacationDeductPercentage { get; set; }
        public int? StartDatetoCalculateAbsence { get; set; }
        public int? StartActivateAttendanceInBalance { get; set; }
        public int? ExtraDaysWillBeAddedToSalry { get; set; }
        public int? AllowMultiCheckIn { get; set; }
        public int? MorningANdEarlyLeavesWillDeductAuto { get; set; }
        public int? BeforeContractPeriod { get; set; }
        public int? LastRefreshBreak { get; set; }
        public int? ViewSocialSecurity { get; set; }
        public int? AddMissingCheckinouttoBalance { get; set; }
        public int? ZKFlag { get; set; }
        public int? MaxOverTimePerDay { get; set; }
        public int? WithHourFraction { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string SalarySlipReportName { get; set; }
        public int? BreakWillDeductFromLeaveInMinutes { get; set; }
        public int? DetailWillBeInShifts { get; set; }
        public int? UseEmployeeWeeklyHoliday { get; set; }
        public int? FridayAllowanceShiftID { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? OvertimeDuringWeekend { get; set; }
        public int? VacationWillDeductFromSalary { get; set; }
        public int? MissingCheckoutWillDeductFromSalary { get; set; }
        public int? NumberOfWorkingHours { get; set; }
        public int? ViewVacationInSalaryReport { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CompanySharePercent3 { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SocialSecurityAmount3 { get; set; }
        public int? cielOvertimeifexceed { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? LoanPercentage { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string HrManager { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string MainBankID { get; set; }
        public int? WithDetailInSalarySlip { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? IsHourRateInShifts { get; set; }
        public int? ByWorkingHours { get; set; }
        public int? DeductCheckinasMissingcheckout { get; set; }
        public int? CompanyMayCLose { get; set; }
        public int? OvertimeHourWillStartAfter { get; set; }
        public int? ViewEmployeeSalaryeport { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string WindowsUserName { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string WindowsUserPassword { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string WindowsDomain { get; set; }
        public int? AdditionalHolidayWillBeViewInSalary { get; set; }
        public int? LeaveWillDeductFromSalaryStartFrom { get; set; }
        public int? ViewFullSalary { get; set; }

        [ForeignKey("ProjectID")]
        [InverseProperty("Settings")]
        public virtual Project Project { get; set; }
    }
}