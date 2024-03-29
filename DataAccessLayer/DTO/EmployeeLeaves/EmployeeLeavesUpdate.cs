﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeLeaves
{
    public class EmployeeLeavesUpdate
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int? LeaveTypeID { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public IFormFile File { get; set; }
    }
}
