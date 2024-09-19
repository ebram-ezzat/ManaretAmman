using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Locations
{
    public class DeleteEmployeeLocation
    {
        [Required(ErrorMessage = "The EmployeeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int EmployeeID { get; set; }
        //[Required(ErrorMessage = "The LocationID is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "The LocationID must be bigger than 0")]
        public int? LocationID { get; set; }
        public int? EmployeeLocationID { get; set; }
    }
}
