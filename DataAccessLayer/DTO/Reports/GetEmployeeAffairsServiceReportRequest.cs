using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeeAffairsServiceReportRequest:ReportBaseModel
    {
        [Required(ErrorMessage = "The EmployeeHRServiceID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeHRServiceID must be bigger than 0")]
        public int EmployeeHRServiceID { get; set; }
        public int? EmployeeID { get; set; }
        /// <summary>
        /// This is the name of Rdlc Report Name 
        /// </summary>
        [Required(ErrorMessage = "The HRServiceReportName is required.")]        
        public string HRServiceReportName { get; set; }
    }
}
