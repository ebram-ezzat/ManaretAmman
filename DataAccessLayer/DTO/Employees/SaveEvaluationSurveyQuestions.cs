using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEvaluationSurveyQuestions
    {
        [Range(1, int.MaxValue, ErrorMessage = "SurveyId is must be bigger than 1")]
        [Required(ErrorMessage = "SurveyId is Required")]
        public int SurveyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CategoryId is must be bigger than 1")]
        [Required(ErrorMessage = "CategoryId is Required")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "QuestionId is must be bigger than 1")]
        [Required(ErrorMessage = "QuestionId is Required")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Question degree is Required")]
        public string QuestionDegree { get; set; }

        public int WithNotes { get; set; }
    }
}
