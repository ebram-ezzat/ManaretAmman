using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeePenaltyReport
    {
        public int? EmployeePenaltyID { get; set; }
        public int? EmployeeID { get; set; }
        public int? PenaltyID { get; set; }
        public int? DepartmentID { get; set; }
        public bool IsExcel { get; set; }
    }
}
