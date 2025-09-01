using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class GetATT_ShiftsInput
    {
        public int? ShiftID { get; set; }
        public string ShiftName { get; set; }
    }
    public class GetATT_ShiftsOutput
    {
        public int? ShiftID { get; set; }
        public int? ShiftTypeID { get; set; }
        public string ShiftName { get; set; }
        public string ShiftTypeIDesc{ get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
    public class GetATT_ShiftsDetailsOutput
    {
        public int? ShiftID { get; set; }
        public int? DetailID { get; set; }
        public int? IsDependinSalary { get; set; }
        public int? IsChecked { get; set; }
        public string FieldValue { get; set; }


    }

}
