using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class EmployeeAttendanceTreatmentInput
    {
        [Required(ErrorMessage = "The ToDate is required.")]
        public DateTime? ToDate { get; set; }
        [Required(ErrorMessage = "The FromDate is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "The Flag is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Flag must be bigger than 0")]
        public int? Flag { get; set; } 
        public int? EmployeeID { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
        public int? ShiftID { get; set; }
        public int? LoginUserID { get; set; }
        public int? DepartmentID { get; set; }
        public int? ApprovalTypeID { get; set; }
    }
    /*
     * غيابات
     * Flag=8
     */
    public class EmployeeAttendanceTreatmentOutputFlag8
    {
        public int? EmployeeID { get; set; }
        public int? AttendanceDate { get; set; }
        public int? CheckIn { get; set; }
        public int? CheckOut { get; set; }
        public int? WeekDayID { get; set; }
        public int? BreakIn { get; set; }
        public int? BreakOut { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftName { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string Notes { get; set; }
        public int? CheckoutAfterMidnight { get; set; }
        public int? IsWorkingDay { get; set; }
        public int? BreakTime { get; set; }
        public int? IsVacation { get; set; }
        public int? AllowedLate { get; set; }
        public int? EarlyLeave { get; set; }
        public int? CalculateAdditionalIfExceed { get; set; }
        public int? MorningAttendanceAsAdditional { get; set; }
        public int? AllowedAdditionalBefore { get; set; }
        public int? FixedDayID { get; set; }
        public int? IsAbsent { get; set; }
        public int? IsLateChecked { get; set; }
        public int? StartTime2 { get; set; }
        public int? EndTime2 { get; set; }
        public string TempCheckinAfter { get; set; }
        public string TempCheckoutAfter { get; set; }
        public string DayDesc { get; set; }
        public int? IsAddedManual { get; set; }
        public int? EmployeeWeeklHolidayStatusID { get; set; }
        public string V_AttendanceDate { get; set; }
    }
    /*
     * الخروج المبكر والتاخير الصباحى
     * Flag=3
     */
    public class EmployeeAttendanceTreatmentOutput
    {
        public int? EmployeeID { get; set; }
        public int? AttendanceDate { get; set; }
        public int? CheckIn { get; set; }
        public int? CheckOut { get; set; }
        public int? WeekDayID { get; set; }
        public int? BreakIn { get; set; }
        public int? BreakOut { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftName { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string Notes { get; set; }
        public int? CheckoutAfterMidnight { get; set; }
        public int? IsWorkingDay { get; set; }
        public int? BreakTime { get; set; }
        public int? IsVacation { get; set; }
        public int? AllowedLate { get; set; }
        public int? EarlyLeave { get; set; }
        public int? CalculateAdditionalIfExceed { get; set; }
        public int? MorningAttendanceAsAdditional { get; set; }
        public int? AllowedAdditionalBefore { get; set; }
        public int? FixedDayID { get; set; }
        public int? IsEarlyLeaveChecked { get; set; }
        public int? IsLateChecked { get; set; }
        public int? StartTime2 { get; set; }
        public int? EndTime2 { get; set; }
        public string TempCheckinAfter { get; set; }
        public string TempCheckoutAfter { get; set; }
        public string DayDesc { get; set; }
        public int? IsAddedManual { get; set; }
        public int? EmployeeWeeklHolidayStatusID { get; set; }
        public string v_AttendanceDate  { get; set; }
       
    }
    /*
    * مغادرات
    * Flag=9
    */
    public class EmployeeAttendanceTreatmentOutputFlag9
    {
        public int? EmployeeID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public int? CheckIn { get; set; }
        public int? CheckOut { get; set; }
        public int WeekDayID { get; set; }
        public int? BreakIn { get; set; }
        public int? BreakOut { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftName { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string Notes { get; set; }
        public int? CheckoutAfterMidnight { get; set; }
        public int? IsWorkingDay { get; set; }
        public int? BreakTime { get; set; }
        public int? IsVacation { get; set; }
        public int? AllowedLate { get; set; }
        public int? EarlyLeave { get; set; }
        public int? CalculateAdditionalIfExceed { get; set; }
        public int? MorningAttendanceAsAdditional { get; set; }
        public int? AllowedAdditionalBefore { get; set; }
        public int? FixedDayID { get; set; }
        public int? IsLeave { get; set; }
        public int? StartTime2 { get; set; }
        public int? EndTime2 { get; set; }
        public string TempCheckinAfter { get; set; }
        public string TempCheckoutAfter { get; set; }
        public string DayDesc { get; set; }
        public int? IsAddedManual { get; set; }
        public int? EmployeeWeeklHolidayStatusID { get; set; }
        public string V_AttendanceDate { get; set; }
    }

}
