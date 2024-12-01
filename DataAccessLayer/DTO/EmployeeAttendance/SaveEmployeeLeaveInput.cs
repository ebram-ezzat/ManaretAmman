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
        public DateTime? LeaveDate { get; set; }// =>attendance date
        /*
         * تاخير صباحى هيبقى starttime
         * الخروج المبكر هتبقى checkout
         * المغادرة9 هتبقى checkin 
         */
        public int? FromTime { get; set; }
        /*
        * تاخير صباحى هيبقى checkin
        *   الخروج المبكر هتبقى endtime
        * المغادرة9 هتبقى checkout 
        */
        public int? ToTime { get; set; }
        public int? CreatedBy { get; set; }
        public int? BySystem { get; set; } = 1;
        public int? PrevilageType { get; set; }
        public string ImagePath { get; set; }
    }
    public class SaveEmployeeVacationInput
    {
        public int? EmployeeVacationID { get; set; }
        public int EmployeeID { get; set; } 
        public int? VacationTypeID { get; set; } 
        public DateTime? FromDate { get; set; } //=>attendance date
        public DateTime? ToDate { get; set; } //=>attendance date
        public string Notes { get; set; }
        public int? DayCount { get; set; } = 1;
        public int? CreatedBy { get; set; } 
        public int? IsCalledFromOtherSP { get; set; } 
        public int? PrevilageType { get; set; } 
        public string ImagePath { get; set; } 
       
    }
}
