using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Contracts;

namespace DataAccessLayer.Models
{
    public class EvaluationSurveyQuestions:IMustHaveProject
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("EvaluationQuestion")]
        public int QuestionId { get; set; }
        public EvaluationQuestion EvaluationQuestion { get; set; }

        [ForeignKey("EvaluationCategory")]
        public int CategoryId { get; set; }
        public EvaluationCategory EvaluationCategory { get; set; }

        public string QuestionDegree { get; set; }
        public int WithNotes { get; set; }


        public int CreatedBy { get; set; }


        public DateTime CreationDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModificationDate { get; set; }

        [ForeignKey("EvaluationSurvey")]
        public int SurveyId { get; set; }
        public EvaluationSurvey EvaluationSurvey { get; set; }
        [NotMapped]
        public int ProjectID { get; set; }

    }
}
