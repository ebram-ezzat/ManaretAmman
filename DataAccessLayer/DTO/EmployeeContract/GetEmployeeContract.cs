using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeContract
{
    public class GetEmployeeContracts : PageModel
    {
        public int? ContractID { get; set; }
        public int? ContractTypeID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? StatusID { get; set; }
        public DateTime? ToFromDate { get; set; }
        public DateTime? ToToDate { get; set; }
    }
    public class GetEmployeeContractsResponse
    {
        public int? ContractID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public int? EmployeeID { get; set; }
        public decimal? Salary { get; set; }
        public int? CreatedBy { get; set; }
        public int? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ModificationDate { get; set; }
        public decimal? SocialSecuritySalary { get; set; }
        public int? ProjectID { get; set; }
        public int? IsDailyWork { get; set; }
        public int? ContractTypeID { get; set; }
        public int? ConfirmationDate { get; set; }
        public string StatusDesc { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public int? StatusID { get; set; }
        public string ContractTypeDesc { get; set; }
        public int? Lastone { get; set; }
        public string ContractTypeValue { get; set; }
        public int? FirstDate { get; set; }
        public DateTime? V_startdate { get; set; }
        public DateTime? V_endDate { get; set; }
        public DateTime? V_FirstDate { get; set; }
        public DateTime? v_ConfirmationDate { get; set; }
    }
}
