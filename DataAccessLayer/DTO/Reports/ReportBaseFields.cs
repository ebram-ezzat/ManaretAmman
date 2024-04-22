using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
   public class ReportBaseModel
    {
        /// <summary>
        /// Default is false that is meaning is a PDF Format 
        /// </summary>
        public bool IsExcel { get; set; } = false;//Default is PDF
    }
    public class ReportBaseFields: ReportBaseModel
    {
       
        /// <summary>
        /// Default is 0 that is meaning is a Date,
        /// 1 that is meaning is an Employee,
        /// 2 that is meaning is an AllEmployess
        /// </summary>
        public int ReportType { get; set; }
        [Required(ErrorMessage = "The FromDate is required.")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "The ToDate is required.")]
        public DateTime? ToDate { get; set; }
        public int? EmployeeID { get; set; }
        public int? DepartmentID { get; set; }
        /// <summary>
        /// Default is 1  
        /// </summary>
        public int Flag { get; set; } = 1;
       
    }
    public enum EnumReportType
    {
        Date,
        Employee,
        AllEmployees
    }
}
