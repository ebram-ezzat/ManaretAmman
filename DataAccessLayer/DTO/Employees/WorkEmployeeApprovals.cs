using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class WorkEmployeeApprovals
    {
        public int? EmployeeID { get; set; }
        public int TypeID { get; set; } = 0;
        public DateTime? AttendanceDate { get; set; }
        public string Systemtimeinminutes { get; set; }
        public string Approvedtimeinminutes { get; set; }
        public int? CreatedBy { get; set; }
        public int? StatusID { get; set; }
    }
}
