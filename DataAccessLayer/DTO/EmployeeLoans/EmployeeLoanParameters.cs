using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeLoans
{
    public class EmployeeLoanParameters:PageModel
    {
        public int? EmployeeLoanID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }       
        public int? LoanTypeID { get; set; }
        public int Flag { get; set; } = 1; // Default value is 1
        public int? CreatedBy { get; set; }
        public int? DepartmentID { get; set; }
        public int? LoanSerial { get; set; }        
       
    }
    //public class EmployeeLoanReturnResult
    //{
    //    public int EmployeeLoanID { get; set; }
    //    public int EmployeeID { get; set; }
    //    public DateTime LoanDate { get; set; }
    //    public decimal LoanAmount { get; set; }
    //    public string Notes { get; set; }
    //    public int CreatedBy { get; set; }
    //    public DateTime CreationDate { get; set; }
    //    public int? ModifiedBy { get; set; }
    //    public DateTime? ModificationDate { get; set; }
    //    public int ProjectID { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string EmployeeNumber { get; set; }
    //    public int EnableDelete { get; set; }
    //    public string StartDate { get; set; } = "d";
    //    public string JobTitleName { get; set; } = "s";
    //    public decimal Salary { get; set; } = 0;
    //    public string LoanTypeDesc { get; set; }
    //    public int LoanTypeID { get; set; }
    //    public decimal CurrentValue { get; set; }
    //    public decimal ScheduledValue { get; set; }
    //    public decimal AllScheduledLoan { get; set; }
    //    public decimal MonthlyLoan { get; set; }
    //    public int IsEnabled { get; set; }
    //    public int CanDelete { get; set; }
    //    public int IsPaid { get; set; }
    //    public DateTime V_LoanDate { get; set; }
    //    public DateTime VV_LoanDate { get; set; }
    //    public string AcceptStatus { get; set; } = "accept";
    //}
}
