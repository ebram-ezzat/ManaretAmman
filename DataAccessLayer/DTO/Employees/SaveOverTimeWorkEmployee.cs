using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveOverTimeWorkEmployee
    {
        public int EmployeeID { get; set; }
        public int? TypeID { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string SystemTimeInMinutes { get; set; }
        public string ApprovedTimeInMinutes { get; set; }       
        public int? StatusID { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Notes { get; set; }
    }
}
