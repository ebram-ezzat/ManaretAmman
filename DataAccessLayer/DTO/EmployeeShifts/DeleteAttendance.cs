using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class DeleteAttendance
    {
        public int? ID { get; set; }
        public int? EmployeeID { get; set; }
    }
}
