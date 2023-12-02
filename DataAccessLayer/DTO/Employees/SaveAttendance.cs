using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveAttendance
    {
        public int? projecId { get; set; }
        public DateTime? attendanceDate { get; set; }
        public int? typeId { get; set; }
        public int? employeeId { get; set; }
        public string macIp { get; set; }
        public string langtitude { get; set; }
        public string latitude { get; set; }
        public int? locationId { get; set; }
    }
}
