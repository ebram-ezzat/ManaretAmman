using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveAllowance_DeductionInput
    {
        public int? AllowanceID { get; set; }
        public string? DefaultDesc { get; set; } 
        public int? TypeID { get; set; } 
        public decimal? Amount { get; set; } 
        public int? CalculateTypeID { get; set; } 
        public int? NatureID { get; set; } 
        public int? IsDefault { get; set; }        
        public int CalculateWithOvertime { get; set; } = 0; 
        public int CalculateWithAttendanceDeduction { get; set; } = 0; 
        public int? IsDependInAttendance { get; set; } 
        public int? IsMonthly { get; set; } 
        public int? DependInCheckout { get; set; } 
        public int? DependInCheckin { get; set; } 

    }
    public class DeleteAllowance_deduction
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The AllowanceID must be bigger than 0")]
        public int AllowanceID { get; set; }
    }
}
