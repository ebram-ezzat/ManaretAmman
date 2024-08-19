using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeTransaction
{
    public class DeleteEmployeeTransaction
    {
        [Required(ErrorMessage = "The EmployeeTransactionID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeTransactionID must be bigger than 0")]
        public int EmployeeTransactionID { get; set; }
    }
}
