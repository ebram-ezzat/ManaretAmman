using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeAllowancesMainScreenOutput
    {
        public int? AllowanceID { get; set; }                      
        public string DefaultDesc { get; set; }                  
        public int? EmployeeAllowanceID { get; set; }             
        public int? EmployeeID { get; set; }                 
        public int? StartDate { get; set; }                
        public int? EndDate { get; set; }                        
        public decimal? Amount { get; set; }                     
        public decimal? AllowanceAmount { get; set; }              
      // public int? CalculateWithOvertime { get; set; }           
        public string WithOverTimeDesc { get; set; }             
        public int? CalculatedWithOverTime { get; set; }         
        public string EmployeeName { get; set; }                  
        public int? EmployeeNumber { get; set; }               
        public DateTime? v_StartDate { get; set; }               
        public DateTime? v_EndDate { get; set; }                  
    }
}
