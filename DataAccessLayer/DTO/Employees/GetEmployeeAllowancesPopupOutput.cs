using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeAllowancesPopupOutput
    {
        public int? EmployeeID { get; set; }                
        public string EmployeeName { get; set; }                
        public int? EmployeeNumber { get; set; }             
        public decimal? Amount { get; set; }                    
        public int? StartDate { get; set; }                  
        public int? EndDate { get; set; }                        
        public int? CalculateWithOvertime { get; set; }          
        public DateTime? v_StartDate { get; set; }           
        public DateTime? v_EndDate { get; set; }               
    }
}
