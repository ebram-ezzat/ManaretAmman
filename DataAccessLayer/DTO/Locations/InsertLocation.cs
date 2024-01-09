using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Locations
{
    public class InsertLocation
    {      
        public string Alias { get; set; } = null;
        public decimal? Distance { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; } 
        public int? CreatedBy { get; set; }
       
    }
    public class UpdateLocation: InsertLocation
    {
        [Required(ErrorMessage = "The LocationID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The LocationID must be bigger than 0")]
        public int LocationID { get; set; }

    }
}
