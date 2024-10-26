using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeTransaction
{
    public class GetEmployeeTransactionOutput
    {
        public int? EmployeeTransactionID { get; set; }
        public int? EmployeeID { get; set; }
        public int? TransactionDate { get; set; }
        public int? TransactionTypeID { get; set; }
        public int? colTransactionTypeID { get; set; }
        public int? TransactionInMinutes { get; set; }
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? BySystem { get; set; }
        public int? RelatedToDate { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public int? EnableDelete { get; set; }
        public string TransactionTypeDesc { get; set; }
        public DateTime? v_transactiondate { get; set; }
    }

    public class EmployeeTransactionAutoResult
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeNumber { get; set; }

        public int TransactionDate { get; set; }

        public string TransactionTypeDesc { get; set; }
        public string Notes { get; set; }

        public decimal? TransactionInMinutes { get; set; }

        public int TransactionTypeID { get; set; }
        public int ColTransactionTypeID { get; set; }
    }

}
