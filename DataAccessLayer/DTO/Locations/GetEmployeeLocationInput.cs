using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Locations
{
    public class GetEmployeeLocationInput:PageModel
    {
        //[Required(ErrorMessage = "The EmployeeID is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int? EmployeeID { get; set; }
        public int? LocationID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
    public class GetEmployeeLocationResponse
    {
        public int? LocationID { get; set; }
        public int? EmployeeID { get; set; }
        public decimal? Distance { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public string Alias { get; set; }
        public int? AnyWhere { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? EmployeeLocationID { get; set; }

    }
}
