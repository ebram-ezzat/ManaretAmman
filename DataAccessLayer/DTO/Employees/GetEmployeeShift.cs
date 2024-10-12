using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeShifts : PageModel
    {
        public string EmployeeID { get; set; }
        public string ShiftID { get; set; }
    }
    public class GetEmployeeShiftsResponse
    {
        public int? EmployeeID { get; set; }
        public int? ShiftID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ShiftName { get; set; }
        public int? IsChecked { get; set; }
    }
    public class GetEmployeeShiftsResponseNagative
    {
        public int? EmployeeID { get; set; }
        public int? ShiftID { get; set; }       
        public string ShiftName { get; set; }
        public int? IsChecked { get; set; }
    }
}
