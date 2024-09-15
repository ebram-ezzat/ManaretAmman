using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeDeductions
{
    public class UpdateEmployeeDeductions
    {
        [Required(ErrorMessage = "The AllowanceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The AllowanceID must be bigger than 0")]
        public int DeductionID { get; set; }
        [Required(ErrorMessage = "The EmployeeAllowanceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeAllowanceID must be bigger than 0")]
        public int? EmployeeAllowanceID { get; set; }
        [Required(ErrorMessage = "The EndDate is required.")]
        public DateTime? EndDate { get; set; }
    }
}
