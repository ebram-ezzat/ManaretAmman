using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeContract
{
    public class SaveEmployeeContracts
    {
        public int? ContractTypeID { get; set; }
        public int? EmployeeID { get; set; }
        public int? StatusID { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? ContractConfirmDate { get; set; }
        public DateTime? ContractFromDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public int? CompanyID { get; set; }
    }
}
