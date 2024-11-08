using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class EmployeeAttendanceTreatmentOutput
    {
        public int? EmployeeID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public int? CheckIn { get; set; }
        public int? CheckOut { get; set; }
        public int? WeekDayID { get; set; }
        public int? BreakIn { get; set; }
        public int? BreakOut { get; set; }
        public int? ProjectID { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftName { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string Notes { get; set; }
        public int? CheckoutAfterMidnight { get; set; }
        public int? IsWorkingDay { get; set; }
        public int BreakTime { get; set; }
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
        public int? TempCheckinAfter { get; set; }
        public int? TempCheckoutAfter { get; set; }
        public string DayDesc { get; set; }
        public int? IsAddedManual { get; set; }
        public int? EmployeeWeeklyHolidayStatusID { get; set; }
        public string FormattedAttendanceDate { get; set; }
        public string FormattedCheckIn { get; set; }
        public string FormattedCheckOut { get; set; }
    }
}
