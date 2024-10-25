using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class SaveEmployeeShiftsExchange
    {
        public int? EmployeeShiftID { get; set; }
        public int? EmployeeID { get; set; }
        public int? ShiftID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? IsCalledFromOtherSP { get; set; }
        public int? WithRefresh { get; set; }
    }
}
