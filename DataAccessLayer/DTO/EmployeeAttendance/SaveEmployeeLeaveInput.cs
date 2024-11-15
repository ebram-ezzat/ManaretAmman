using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class SaveEmployeeLeaveInput
    {
        public int? EmployeeLeaveID { get; set; }
        public int? EmployeeID { get; set; }
       // public int? LeaveTypeID { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int? CreatedBy { get; set; }
        public int? BySystem { get; set; }
        public int? PrevilageType { get; set; }
        public string ImagePath { get; set; }
    }
    public class SaveEmployeeVacationInput
    {
        public int? EmployeeVacationID { get; set; }
        public int EmployeeID { get; set; } 
        public int? VacationTypeID { get; set; } 
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; } 
        public string Notes { get; set; } 
        public int? DayCount { get; set; } 
        public int? CreatedBy { get; set; } 
        public int ProjectID { get; set; } 
        public int IsCalledFromOtherSP { get; set; } 
        public int? PrevilageType { get; set; } 
        public string ImagePath { get; set; } 
       
    }
}
