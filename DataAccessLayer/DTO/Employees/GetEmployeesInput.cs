using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeesInput:PageModel
    {
        public int? StatusID { get; set; }
        public int? EmployeeID { get; set; }
        public string Search { get; set; }
        public int? SupervisorID { get; set; }      
        public int? CreatedBy { get; set; }
        public int? DepartmentID { get; set; }
        public int? YearID { get; set; }
    }
}
