using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeTransaction
{
    public class SaveEmployeeTransaction
    {
        public int? EmployeeTransactionID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? TransactionTypeID { get; set; }
        public int? TransactionInMinutes { get; set; }
        public string Notes { get; set; }
        public int? BySystem { get; set; }
        public int? RelatedToDate { get; set; }
        public int? ByPayroll { get; set; }
    }
}
