using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeIncrease
{
    public class GetEmployeeIncreaseInput : PageModel
    {
        public int? DetailID { get; set; }
        public int? EmployeeID { get; set; }
        public int? Flag { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DepartmentID { get; set; }
        public int? LoginUserID { get; set; }
    }
    public class GetEmployeeIncreaseOutput
    {
        public int? DetailID { get; set; }
        public int? EmployeeID { get; set; }
        public int? IncreaseDate { get; set; }
        public decimal? Amount { get; set; }
        public int? LastAction { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? Salary { get; set; }
        public decimal? SocialSecuritySalary { get; set; }
        public decimal? SSNAmount { get; set; }
        public int? SSSIncreseDate { get; set; }
        public DateTime? v_SSSIncreseDate { get; set; }
        public DateTime? v_IncreaseDate { get; set; }
    }
}
