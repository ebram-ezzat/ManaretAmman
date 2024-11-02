using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeeRatingDetailsInput
    {
        public int? EmployeeID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The EvaluationID must be bigger than 0")]
        public int EvaluationID { get; set; }
        [Required( ErrorMessage = "The EvaluationDate Required")]
        public DateTime? EvaluationDate { get; set; }
        public int StatusID { get; set; } 
        public string QuestionID { get; set; }  
        public string Values { get; set; }     
        public string Notes { get; set; }       // Corrected typo to "Notes" from "Notess"        
        public int? EvaluationEmployeeID { get; set; }  // Output parameter

    }
}
