using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEvaluationSurveyQuestions
    {
        [Key]
        public int Id { get; set; }
        
        public int QuestionId { get; set; }
        public string EvaluationQuestion { get; set; }
        
        public int CategoryId { get; set; }
        public string EvaluationCategoryName { get; set; }

        public string QuestionDegree { get; set; }
        public int WithNotes { get; set; }

        [Required(ErrorMessage = "The SurveyId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The SurveyId must be bigger than 0")]
        public int SurveyId { get; set; }
        public string EvaluationSurveyName { get; set; }
    }
}
