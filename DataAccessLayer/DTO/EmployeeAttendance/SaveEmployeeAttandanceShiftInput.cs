using DataAccessLayer.DTO.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class SaveEmployeeAttandanceShiftInput
    {
        public int? EmployeeShiftID { get; set; } // OUTPUT parameter
        [ListIntNotEmpty(ErrorMessage = "The EmployeeID list must contain at least one item.")]
        public List<int> EmployeeIDs { get; set; }
        [Required(ErrorMessage = "The ShiftID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The ShiftID must be bigger than 0")]
        public int? ShiftID { get; set; }
        [Required(ErrorMessage = "The FromDate is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "The ToDate is required.")]
        public DateTime? ToDate { get; set; }
        //public int? CreatedBy { get; set; }
        //public int IsCalledFromOtherSP { get; set; } = 0; // Default value
        //public int? WithRefresh { get; set; }
    }
    
}
