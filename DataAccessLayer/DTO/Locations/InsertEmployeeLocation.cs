using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Locations
{
    public class InsertEmployeeLocation
    {
        public int? LocationID { get; set; }
        public int? EmployeeID { get; set; }
        public decimal? Distance { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }       
        public int ?AnyWhere { get; set; }
    }
}
