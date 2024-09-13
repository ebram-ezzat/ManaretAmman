using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class DeleteEmployeeWithRelatedData
    {
        [Required(ErrorMessage = "The EmployeeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeId must be bigger than 0")]
        public int EmployeeId { get; set; }
    }
}
