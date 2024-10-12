using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeAdditionalInfoInput
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]

        public int EmployeeID { get; set;}
    }
}
