﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeTransaction
{
    public class GetEmployeeTransactionInput : PageModel 
    {
        public int? EmployeeTransactionID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Flag { get; set; } = 1;
        public int? TransactionTypeID { get; set; }
        public int? DepartmentID { get; set; }
    }
}
