using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeeSalaryReportRequestV2
    {       
        public int? EmployeeID { get; set; } = null;
        public int? CurrentYearID { get; set; } = null;
        public int? CurrentMonthID { get; set; } = null;
        public int IsAllEmployees { get; set; } = 0;
        public int WithDetail { get; set; } = 0;    
       
        public int Flag { get; set; } = 1;      
        
    }
}
