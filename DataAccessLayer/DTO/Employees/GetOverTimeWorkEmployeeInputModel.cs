using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetOverTimeWorkEmployeeInputModel:PageModel
    {
        public int? EmployeeID { get; set; }
        public int? TypeID { get; set; }
        public int? FromDate { get; set; }
        public int? ToDate { get; set; }
        public int? ProjectID { get; set; }          
        public int? LoginUserID { get; set; }
        public int LanguageID { get; set; }=1 ;
        public int? RowCount { get; set; }
    }
}
