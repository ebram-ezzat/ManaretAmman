using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class ATT_SaveShift
    {
        public int? ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int? ShiftTypeID { get; set; }
        public int? CreatedBy { get; set; }
       
    }
}
