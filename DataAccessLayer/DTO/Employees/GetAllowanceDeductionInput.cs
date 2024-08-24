using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetAllowanceDeductionInput
    {
        public int? AllowanceID { get; set; }
        public int NatureID { get; set; } = 1;
        public string Search { get; set; }
    }
}
