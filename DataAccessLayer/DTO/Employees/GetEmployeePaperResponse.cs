using DataAccessLayer.DTO.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeePaperResponse
    {
        public int? EmployeeID { get; set; }
        public int? PaperID { get; set; }
        public int? DetailID { get; set; }
        public string PaperPath { get; set; }
        public string Notes { get; set; }
        public int? createdby { get; set; }
        public int? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate{get;set;}
        public string EmployeeName { get; set; }
		public int? EmployeeNumber { get; set; }
		public string PaperDesc { get; set; }
        public int? enabledelete { get; set; }
    }
    public class GetEmployeePaperRequest: PageModel
    {
        public int? EmployeeID { get; set; }
        public int? DetailID { get; set; }
        [Required(ErrorMessage = "The Flag is required.")]
        [Range(1, 3, ErrorMessage = "The Flag must be between 1 and 3.")]
        public int Flag { get; set; }
        public int? LanguageID { get; set; }
        public int? PaperID { get; set; }
        [CustomValidationProjectId]
        public int ProjectID { get; set; }     
            
                
        [CustomValidationLoginUserID]
		public int? LoginUserID { get; set; }
    }
}
