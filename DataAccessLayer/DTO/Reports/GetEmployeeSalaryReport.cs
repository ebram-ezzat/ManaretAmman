using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeeSalaryReport
    {
        public int? EmployeeID { get; set; }
        public int? CurrentYearID { get; set; }
        public int? CurrentMonthID { get; set; }
        public int? IsAllEmployees { get; set; }
        public int? Flag { get; set; }
        public int? TypeID { get; set; }
        public bool IsExcel { get; set; }
    }
}
