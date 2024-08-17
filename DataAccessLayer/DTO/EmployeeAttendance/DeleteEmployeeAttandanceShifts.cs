using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class DeleteEmployeeAttandanceShifts
    {
        [Required(ErrorMessage = "The EmployeeShiftID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeShiftID must be bigger than 0")]
        public int EmployeeShiftID { get; set; }
    }
}
