using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class GetEmployeeAttandanceShiftInput
    {
        public int? EmployeeShiftID { get; set; }
        public int? EmployeeID { get; set; }
        [Required(ErrorMessage = "The FromDate is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "The ToDate is required.")]

        public DateTime? ToDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
