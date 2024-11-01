using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeRatingInput
    {
       public int? EmployeeID { get; set; }
	   public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }
       public int Flag { get; set; } = 1;
       public int? DepartmentID  { get; set; }
       public int? StatusID   { get; set; }
    }
    public class GetEmployeeRatingOutput
    {
        public int? EmployeeEvaluationID { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public string v_StartDate { get; set; } 
        public string v_EndDate { get; set; }
        public string v_Period { get; set; } 
        public string DepartmentName { get; set; }
        public string EmployeeLevelDesc { get; set; }
        public string JobTitleName { get; set; }
        public string v_EvalFromDate { get; set; } 
        public string v_EvalToDate { get; set; } 
        public string EvaluationName { get; set; }
        public string v_EvaluationDate { get; set; } 
        public int? EvalueationPoints { get; set; } 
        public string EvaluationStatus { get; set; }
        public int? NextApprovalStatusID { get; set; } 
        public int? AllowEdit { get; set; }
        public int? StatusID { get; set; }
    }
}
