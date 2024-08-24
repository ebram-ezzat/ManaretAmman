using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeeAllowances
    {
        public int? EmployeeAllowanceID { get; set; }
        [Required(ErrorMessage = "The EmployeeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int? EmployeeID { get; set; }               
        public DateTime? StartDate { get; set; }               
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "The AllowanceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The AllowanceID must be bigger than 0")]
        public int? AllowanceID { get; set; }       
                  
        public decimal? Amount { get; set; }    
        /// <summary>
        /// 0 not checked ,1 checked
        /// </summary>
        public int? CalculatedWithOverTime { get; set; }   
                    
    }
}
