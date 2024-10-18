using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeeTransactionAutoInput
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int? EmployeeID { get; set; }
        public DateTime? TransactionDate { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The TransactionTypeID must be bigger than 0")]
        public int? TransactionTypeID { get; set; }
        public int? TransactionInMinutes { get; set; }
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        public int? BySystem { get; set; }
        public int? RelatedToDate { get; set; }
       
    }
}
