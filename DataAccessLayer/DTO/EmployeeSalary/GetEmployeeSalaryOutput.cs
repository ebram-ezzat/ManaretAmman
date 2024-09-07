using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeSalary
{
    public class GetEmployeeSalaryOutput
    {
        public int? EmployeeID { get; set; }
        public int? CurrentYearID { get; set; }
        public int? CurrentMonthID { get; set; }
        public int? TypeID { get; set; }
        public int? SubTypeID { get; set; }
        public decimal? Amount { get; set; }
        public int? FromDate { get; set; }
        public int? ToDate { get; set; }
        public int? StatusID { get; set; }
        public int? CalculationDate { get; set; }
        public int? AdditionalInWork { get; set; }
        public int? AdditionalInHoliday { get; set; }
        public decimal? morninglate { get; set; }
        public int? workingdays { get; set; }
        public int? monthdays { get; set; }
        public int? missingcheckin { get; set; }
        public int? missingcheckout { get; set; }
        public int? ProjectID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string StatusDesc { get; set; }
        public string SUBTYpeDesc { get; set; }
        public int? IsPublish { get; set; }
        public string emailcontent { get; set; }
        public string emailsubject { get; set; }
        public DateTime? V_CalculationDate { get; set; }
    }
}
