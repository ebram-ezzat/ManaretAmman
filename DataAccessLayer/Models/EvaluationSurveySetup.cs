using DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class EvaluationSurveySetup: IMustHaveProject
    {
        public int Id { get; set; }
        public string DepartmentIds { get; set; }
        public string EmployeelevelIds { get; set; }
        public string UsertypeData { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int StatusId { get; set; }
        [ForeignKey("Projects")]
        public int ProjectID { get; set; }
        public Project Projects { get; set; }
        [ForeignKey("EvaluationSurvey")]
        public int SurveyId { get; set; }
        public EvaluationSurvey EvaluationSurvey { get; set; }
        
        public int CreatedBy { get; set; }


        public DateTime CreationDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModificationDate { get; set; }

    }
}
