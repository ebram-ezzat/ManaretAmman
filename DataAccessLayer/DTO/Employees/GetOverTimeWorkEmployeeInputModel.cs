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
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ProjectID { get; set; }          
        public int? LoginUserID { get; set; }
        public int? LanguageID { get; set; }=1 ;
        
    }
}
