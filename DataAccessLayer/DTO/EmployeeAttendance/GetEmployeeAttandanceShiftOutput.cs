using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class GetEmployeeAttandanceShiftOutput
    {
        public int? EmployeeShiftID { get; set; }
        public int? EmployeeID { get; set; }
        public int? ShiftID { get; set; }
        public int? FromDate { get; set; }
        public int? ToDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public int? EnableDelete { get; set; }
        public string ShiftName { get; set; }
        public DateTime? v_FromDate { get; set; }
        public DateTime? v_ToDate { get; set; }
    }
}
