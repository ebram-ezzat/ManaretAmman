using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Locations
{
    public class GetLocationsInput:PageModel
    {
        public string Alias { get; set; }     
    }
    public class GetLocationsResponse 
    {
        public int? LocationID { get; set; }
        public int? ProjectID { get; set; }
        public string? Alias { get; set; }
        public decimal? Distance { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
