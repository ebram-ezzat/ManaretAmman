using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeVacations
{
    public class OfficialVacationSaveData
    {
        public int? HolidayID { get; set; }
        [Required(ErrorMessage = "The HolidayTypeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The HolidayTypeID must be bigger than 0")]
        public int? HolidayTypeID { get; set; }
        [Required(ErrorMessage = "The FromDate is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "The ToDate is required.")]

        public DateTime? ToDate { get; set; }
        public string Notes { get; set; }
    }
}
