using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class ATT_SaveShiftDetails
    {
        public int? ShiftID { get; set; }
        public int? DetailID { get; set; }
        public int? IsDependinSalary { get; set; }
        public int? IsChecked { get; set; }
        public string FieldValue { get; set; }
        
    }
}
