using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeRatingDetailsInput
    {
       public int? EmployeeID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The EvaluationID must be bigger than 0")]
        public int EvaluationID { get; set; }
    }
    public class GetEmployeeRatingDetailsOutput
    {
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public string v_StartDate { get; set; }  
        public string v_EndDate { get; set; }   
        public string v_Period { get; set; }        
        public string DepartmentName { get; set; }
        public string EmployeeLevelDesc { get; set; }
        public string JobTitleName { get; set; }
        public string EvalFromDate { get; set; }  
        public string EvalToDate { get; set; }   
        public string EvaluationName { get; set; }
        public string v_EvaluationDate { get; set; } 
        public int? EvaluationPoints { get; set; } 
        public string EvaluationStatus { get; set; }
        public string Question { get; set; }
        public string CategoryName { get; set; }
        public int? QuestionID { get; set; }
        public int? CategoryID { get; set; }
        public int? WithNotes { get; set; }
        public string Notes { get; set; }
        public string QuestionDegree { get; set; }
        public decimal? Amount { get; set; }
    }
}
