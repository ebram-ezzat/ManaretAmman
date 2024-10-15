using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeIncrease
{
    public class SaveEmployeeIncrease
    {
        public int? EmployeeID { get; set; }
        public DateTime? IncreaseDate { get; set; }
        public DateTime? SSSIncreaseDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? SSNAmount { get; set; }
    }
}
