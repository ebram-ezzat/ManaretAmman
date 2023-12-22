using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetOverTimeWorkEmployeeOutputModel
    {
        public int? EmployeeID { get; set; }
        public int? TypeID { get; set; }
        public int? AttendanceDate { get; set; }
        public int? SystemTimeInMinutes { get; set; }
        public int? ApprovedTimeInMinutes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? StatusID { get; set; }
        public int? ToTime { get; set; }
        public int? ActionTypeID { get; set; }
        public int? EmployeeApprovalID { get; set; }
        public string Notes { get; set; }
        public int? FromTime { get; set; }
        public string EmployeeName { get; set; }
        public int? CheckIn { get; set; }
        public int? CheckOut { get; set; }
        public string StatusDesc { get; set; }
        public int? EmployeeNumber { get; set; }
    }
}
