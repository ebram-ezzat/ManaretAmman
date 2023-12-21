using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetOverTimeWorkEmployeeOutputModel
    {
        public int EmployeeID { get; set; }
        public int TypeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int SystemTimeInMinutes { get; set; }
        public int ApprovedTimeInMinutes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public int StatusID { get; set; }
        public DateTime ToTime { get; set; }
        public int ActionTypeID { get; set; }
        public int EmployeeApprovalID { get; set; }
        public string Notes { get; set; }
        public DateTime FromTime { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string StatusDesc { get; set; }
        public string EmployeeNumber { get; set; }
    }
}
