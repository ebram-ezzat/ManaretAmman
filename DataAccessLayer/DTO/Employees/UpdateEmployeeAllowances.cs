using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class UpdateEmployeeAllowances
    {
        [Required(ErrorMessage = "The AllowanceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The AllowanceID must be bigger than 0")]
        public int AllowanceID { get; set; }
        [Required(ErrorMessage = "The EmployeeAllowanceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeAllowanceID must be bigger than 0")]
        public int? EmployeeAllowanceID { get; set; }
        [Required(ErrorMessage = "The EndDate is required.")]
        public DateTime? EndDate { get; set; }
    }
}
