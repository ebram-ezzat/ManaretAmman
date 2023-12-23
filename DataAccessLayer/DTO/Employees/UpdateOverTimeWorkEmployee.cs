using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class UpdateOverTimeWorkEmployee
    {
        public int EmployeeID { get; set; }
        public int? TypeID { get; set; }
        [Required(ErrorMessage = "The AttendanceDate is required.")]
        public DateTime? AttendanceDate { get; set; }
        public string SystemTimeInMinutes { get; set; }
        public string ApprovedTimeInMinutes { get; set; }
        public int? StatusID { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "The EmployeeApprovalID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeApprovalID must be bigger than 0")]
        public int EmployeeApprovalID { get; set; }
        public int ActionTypeID { get; set; }
    }
}
