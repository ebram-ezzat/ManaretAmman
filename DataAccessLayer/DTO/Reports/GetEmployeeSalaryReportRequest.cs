using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeeSalaryReportRequest
    {
        public int? ProjectID { get; set; }
        public int? EmployeeID { get; set; }
        public int? StatusID { get; set; }
        public int TypeID { get; set; } = 0;
        public int? CurrentYearID { get; set; }
        public int? CurrentMonthID { get; set; }
        public int LanguageID { get; set; } = 1;
        public int? DepartmentID { get; set; }
        public int? DailyWork { get; set; }
        public int? loginuserid { get; set; }
    }
    public class GetEmployeeSalaryReportResponse
    {
        public int EmployeeID { get; set; }
        public int CurrentYearID { get; set; }
        public int CurrentMonthID { get; set; }
        public int TypeID { get; set; }
        public int SubTypeID { get; set; }
        public decimal Amount { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
        public int StatusID { get; set; }
        public int CalculationDate { get; set; }
        public int EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string StatusDesc { get; set; }
        public string SubTypeDesc { get; set; }
        public int IsPublish { get; set; }
        public string EmailContent { get; set; }
        public string EmailSubject { get; set; }
    }
}
