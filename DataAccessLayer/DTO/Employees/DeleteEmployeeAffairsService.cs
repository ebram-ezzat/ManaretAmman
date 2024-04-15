using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class DeleteEmployeeAffairsService
    {
        [Required(ErrorMessage = "The EmployeeHRServiceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeHRServiceID must be bigger than 0")]
        public int EmployeeHRServiceID { get; set; }
        [Required(ErrorMessage = "The EmployeeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int EmployeeID { get; set; }
        public int StatusID { get; set; }// 2 Deleted  , 1 active , 0 archived
      
    }
}
