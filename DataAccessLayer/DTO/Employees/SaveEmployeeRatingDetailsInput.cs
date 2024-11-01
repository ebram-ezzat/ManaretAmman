using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeeRatingDetailsInput
    {
        public int? EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public int EvaluationID { get; set; }
        public int EvaluationDate { get; set; }
        public int CreatedBy { get; set; }
        public int StatusID { get; set; }
        public string QuestionID { get; set; }  
        public string Values { get; set; }     
        public string Notes { get; set; }       // Corrected typo to "Notes" from "Notess"        
        public int? EvaluationEmployeeID { get; set; }  // Output parameter

    }
}
