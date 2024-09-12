using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeSalary
{
    public class GetEmployeeSalaryDetailsOutput
    {
        public decimal? NetSalary { get; set; }
        public decimal? Allowances { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? advances { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? OverTime { get; set; }

        public List<Tuple<string, decimal?>> AllowancesTable = new List<Tuple<string, decimal?>>();
        public List<Tuple<string, decimal?>> DeductionsTable = new List<Tuple<string, decimal?>>();
    }
}
