using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Lookup
{
    /// <summary>
    /// All Fields is required 
    /// </summary>
    public class InsertLookup
    {
        public int? ID { get; set; } = null;
        [Required(ErrorMessage = "TableName is Required")]
        public string TableName { get; set; }
        [Required(ErrorMessage = "ColumnName is Required")]
        public string ColumnName { get; set; }
        [Required(ErrorMessage = "ColumnValue is Required")]
        public string ColumnValue { get; set; } 
        [Required(ErrorMessage = "ColumnDescription is Required")]
        public string ColumnDescription { get; set; }
        [Required(ErrorMessage = "ColumnDescriptionAR is Required")]
        public string ColumnDescriptionAR { get; set; }
        [Required(ErrorMessage = "OrderBy is Required")]

        public int? OrderBy { get; set; } 
        public int ProjectID { get; set; }
        //public int? Balance { get; set; } = null;
        //public int? DefaultValue { get; set; } = null;
        //public int? ParentID { get; set; } = null;
       
        //public int? EmployeeID { get; set; } = null;
        //public int? CalWithHoliday { get; set; } = null;
        //public int? IsHealthVacation { get; set; } = null;
        //public int? IsInjuryVacation { get; set; } = null;       
        //public int? ApprovalProcessID { get; set; } = null;
        //public int? FirstPeriod { get; set; } = null;
        //public int? SecondPeriod { get; set; } = null;
        //public int? ThirdPeriod { get; set; } = null;
        //public int? FourthPeriod { get; set; } = null;
        //public int? FifthPeriod { get; set; } = null;
        //public int? PenaltyCategoryID2 { get; set; } = null;
        //public int? PenaltyCategoryID3 { get; set; } = null;
        //public int? PenaltyCategoryID4 { get; set; } = null;
        //public int? PenaltyCategoryID5 { get; set; } = null;
        //public int? IsWithoutSalaryVacation { get; set; } = null;
        //public int? IsPersonalVacation { get; set; } = null;
        //public decimal? WithoutSalaryVacationValue { get; set; } = null;
        //public int? IsOtherVacation { get; set; } = null;
    }
}
