using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeePenalty
    {
        public int? EmployeePenaltyID { get; set; }
        public int? EmployeeID { get; set; }
        public int? PenaltyID { get; set; }
        public int? DayCount { get; set; }
        public DateTime? PenaltyDate { get; set; }
        public string ReasonDesc { get; set; }
        public int? CreatedBy { get; set; }
        public int? StatusID { get; set; }
        public int? AppliedPenaltyCategoryTypeID { get; set; }
    }
}
