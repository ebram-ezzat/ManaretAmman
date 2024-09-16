﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeDeductions
{
    public class GetEmployeeDeductionsInput : PageModel
    {
        public int? EmployeeDeductionID { get; set; }
        public int? EmployeeID { get; set; }
        public int? NatureID { get; set; }
        public int LanguageID { get; set; } = 1;
        public int Flag { get; set; } = 1;
        public int? AllowanceID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DepartmentID { get; set; }
    }
}