using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeePenalty : PageModel
    {
        public int? EmployeePenaltyID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? StatusID { get; set; }
        public int? PenaltyID { get; set; }
    }
    public class GetEmployeePenaltyResponse
    {
        public int? EmployeePenaltyID { get; set; }
        public int? EmployeeID { get; set; }
        public int? PenaltyID { get; set; }
        public int? DayCount { get; set; }
        public int? PenaltyDate { get; set; }
        public string ReasonDesc { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string PenaltyDesc { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public int? StatusID { get; set; }
        public string StatusDesc { get; set; }
        public int? EnableDelete { get; set; }
        public string PenaltyCategory { get; set; }
        public int? AppliedPenaltyCategoryTypeID { get; set; }
        public DateTime? v_PenaltyDate { get; set; }
    }
}
