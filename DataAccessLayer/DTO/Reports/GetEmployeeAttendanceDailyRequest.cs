using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeeAttendanceDailyRequest: ReportBaseFields
    {
       
       
    }
    public class GetEmployeeAttendanceDailyResponse
    {
        public int? a { get; set; }
      
        public int? EmployeeID { get; set; }
        public int? AttendanceDate { get; set; }
        public string CheckIn { get; set; }
        public string Checkout { get; set; }
        public string BreakIn { get; set; }
        public string BreakOut { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? EnableDelete { get; set; }
        public int? EnableUpdate { get; set; }
        public int? DayID { get; set; }
        public int? IsWorkingDay { get; set; }
        public string CurrDate { get; set; }
        public string Notes { get; set; }
        public string companyname { get; set; }
        public string footertitle1 { get; set; }
        public string footertitle2 { get; set; }
        public string imagepath { get; set; }
        public string noofhours { get; set; }
        public string allhours { get; set; }
        public string AllExpectedhours { get; set; }
        public string v_FromDate { get; set; }
        public string v_ToDate { get; set; }
        public string allhoursAdditionalINWork { get; set; }
        public string allhoursAdditionalINHoliday { get; set; }
        public int? NoOfHoliday { get; set; }
        public string AllAdditionalHours { get; set; }
        public string alladditionalhoursbysystem { get; set; }
        public string DayDesc { get; set; }
        public string AllLate { get; set; }
        public string ShiftName { get; set; }
        public string DepartmentDesc { get; set; }
        public string V_DepartmentDesc { get; set; }
        public string ActualWorkingHours { get; set; }
        public string ADDitinalHoursInDay { get; set; }
        public string WorkingHoursWithoutbreak { get; set; }
        public string calculatedHours { get; set; }
        public string AllLeavesLate { get; set; }
        public string allMorningLate { get; set; }
        public string allMorningLatebysystem { get; set; }
        public string allEarlyLate { get; set; }
        public string EarlyLate { get; set; }
        public int? inminutesIsabsent { get; set; }
        public int? WithBreak { get; set; }
        public string LeavesLate { get; set; }
        public string MorningLate { get; set; }
        public string BreakTime { get; set; }
        public string allBreakTime { get; set; }
        public string allTotalHours { get; set; }
        public int? ShiftWorkingDay { get; set; }
        public string Period2checkIn { get; set; }
        public string Period2Checkout { get; set; }
        public string TimeDr { get; set; }
        public string approvedtimeinholiday { get; set; }
        public int? actualovertimeinholiday { get; set; }
    }
}
