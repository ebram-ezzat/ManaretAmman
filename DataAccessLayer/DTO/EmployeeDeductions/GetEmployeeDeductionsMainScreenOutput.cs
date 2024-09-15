using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeDeductions
{
    public class GetEmployeeDeductionsMainScreenOutput
    {
        public int? AllowanceID { get; set; }
        public string DefaultDesc { get; set; }
        public int? EmployeeDeductionID { get; set; }
        public int? EmployeeID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DeductionAmount { get; set; }         
        //public string WithOverTimeDesc { get; set; }
        //public int? CalculatedWithOverTime { get; set; }
        public string EmployeeName { get; set; }
        //public int? EmployeeNumber { get; set; }
        public DateTime? v_StartDate { get; set; }
        public DateTime? v_EndDate { get; set; }
    }
}
