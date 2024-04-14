using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeeAffairsServices
    {
        public int? EmployeeHRServiceID { get; set; }
        [Required(ErrorMessage = "The EmployeeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int? EmployeeID { get; set; }
        [Required(ErrorMessage = "The HRServiceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The HRServiceID must be bigger than 0")]
        public int? HRServiceID { get; set; }
        public int? MonthID { get; set; } 
        public DateTime? HRServiceDate { get; set; } 
       // public string ReasonDesc { get; set; } 
      
        /// <summary>
        /// should be send by 1
        /// </summary>
        public int? StatusID { get; set; } 
        public int? YearID { get; set; }
        /// <summary>
        ///not Required
        /// </summary>
        public int? BankID { get; set; }
        /// <summary>
        ///not Required
        /// </summary>
        public int? BranchID { get; set; }
        //public string ServiceText { get; set; } 
        /// <summary>
        ///not Required
        /// </summary>
        public string AttachmentDesc { get; set; }

        public string Notes { get; set; }
    }
}
