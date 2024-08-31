using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeSalary
{
    public class GetEmployeeSalaryInput
    {
        public int? EmployeeID { get; set; }
        public int? StatusID { get; set; }
        public int? TypeID { get; set; }
        public int? CurrentYearID { get; set; }
        public int? CurrentMonthID { get; set; }
        public int? DepartmentID { get; set; }
        public int? DailyWork { get; set; }
    }
}
