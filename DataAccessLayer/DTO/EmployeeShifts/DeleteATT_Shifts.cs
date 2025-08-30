using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class DeleteATT_Shifts
    {
        [Required(ErrorMessage = "The ShiftID Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The ShiftID must be bigger than 0")]
        public int ShiftID { get; set; }
    }
}
