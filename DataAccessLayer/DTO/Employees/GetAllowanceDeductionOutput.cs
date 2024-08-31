using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetAllowanceDeductionOutput
    {
        public int AllowanceID { get; set; }                 
        public string DefaultDesc { get; set; }              
        public int? TypeID { get; set; }                     
        public decimal? Amount { get; set; }                
        public int? CalculateTypeID { get; set; }             
        public int? NatureID { get; set; }                   
        public int? IsDefault { get; set; }                 
        public int? CreatedBy { get; set; }                  
        public DateTime? CreationDate { get; set; }          
        public int? ModifiedBy { get; set; }                
        public DateTime? ModificationDate { get; set; }     
        public int? ProjectID { get; set; }                   
        public int? EnableDelete { get; set; }   
        public int? CalculateWithOvertime { get; set; }     
        public int? CalculateWithAttendanceDeduction { get; set; } 
        public int? IsDependInAttendance { get; set; }       
        public int? IsMonthly { get; set; }                 
    }
}
