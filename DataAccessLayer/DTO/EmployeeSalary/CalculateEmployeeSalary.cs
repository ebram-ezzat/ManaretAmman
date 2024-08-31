using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeSalary
{
    public class CalculateEmployeeSalary
    {
        public int? EmployeeID { get; set; }
        public int? CurrentMonthID { get; set; }
        public int? CurrentYearID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DepartmentID { get; set; }
        public int? DailyWork { get; set; }
    }
}
