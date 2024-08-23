using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeVacations
{
    public class OfficialVacationGetInput:PageModel
    {
        public int? HolidayID { get; set; }
        [Required(ErrorMessage = "The FromDate is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "The ToDate is required.")]
        public DateTime? ToDate { get; set; }        
     
        
    }
}
