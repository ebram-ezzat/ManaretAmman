using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeShifts
{
    public class InsertAttendance
    {
        public DateTime? AttDate { get; set; }
        public string Type { get; set; }
        public int? EmployeeID { get; set; }
        public int? StatusID { get; set; }
        public int? attdateint { get; set; }
        public DateTime? Datetime { get; set; }
    }
}
