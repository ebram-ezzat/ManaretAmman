﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class WorkEmployeeApprovals
    {
        public int EmployeeID { get; set; }
        public int TypeID { get; set; }
        public int AttendanceDate { get; set; }
        public int Systemtimeinminutes { get; set; }
        public int Approvedtimeinminutes { get; set; }
        public int CreatedBy { get; set; }
        public int StatusID { get; set; }
    }
}
