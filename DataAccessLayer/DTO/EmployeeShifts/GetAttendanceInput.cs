using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class GetAttendanceInput
    {
        public int? ID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? EmployeeID { get; set; }
        public int? Flag { get; set; }
        public int? DepartmentID { get; set; }
        public int? OnlyFromDevice { get; set; }
    }

    public class GetAttendanceOutput
    {
        public int? ID { get; set; }
        public string AttDate { get; set; }
        public int? EmployeeID { get; set; }
        public int? Type { get; set; }
        public int? StatusID { get; set; }
        public string attTIme { get; set; }
        public string TypeDesc { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }
    }
}
