using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeDeductions
{
    public class DeleteEmployeeDeductions
    {
        [Required(ErrorMessage = "The EmployeeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "The EmployeeDeductionID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeDeductionID must be bigger than 0")]
        public int? EmployeeDeductionID { get; set; }
    }
}
