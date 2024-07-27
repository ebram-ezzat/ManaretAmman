using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeLoans
{
    public class DeleteSchededuledLoansInput
    {
        public int? EmployeeLoanID { get; set; }
		public int? LoanSerial { get; set; }
        public int? EmployeeID { get; set; }
    }
}
