
namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class EmployeeAttendanceOutput
    {
        public int? EmployeeID { get; set; }

        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }

        public string DayDesc { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string ShiftName { get; set; }
        public string Workhours { get; set; }
        public string ShiftWithTimes { get; set; }
        public string Systemtimeinminutes { get; set; }
        public string Approvedtimeinminutes { get; set; }
        public int? ApprovedStatusID { get; set; }
        public string EmployeeImage { get; set; }
        public string JobTitleName { get; set; }
        public int? AnyWhere { get; set; }
    }
}
