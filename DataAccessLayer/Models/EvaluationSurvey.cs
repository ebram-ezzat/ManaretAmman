using DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class EvaluationSurvey : IMustHaveProject
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }

        public string Notes { get; set; }

        public int StatusId { get; set; }


        public int CreatedBy { get; set; }


        public DateTime CreationDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModificationDate { get; set; }

        [ForeignKey("Projects")]
        public int ProjectID { get; set; }
        public Project Projects { get; set; }
        public ICollection<EvaluationSurveyQuestions> EvaluationSurveyQuestions { get; set; }
        public ICollection<EvaluationSurveySetup> EvaluationSurveySetup { get; set; }


    }
}
