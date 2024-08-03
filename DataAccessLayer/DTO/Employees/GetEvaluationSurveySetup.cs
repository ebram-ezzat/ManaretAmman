using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEvaluationSurveySetup
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The SurveyId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The SurveyId must be bigger than 0")]
        public int SurveyId { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> EmployeelevelIds { get; set; }
        public List<UserTypeEvaluationSurveySetup> UsertypeData { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int StatusId { get; set; } = 1;
        public int CreatedBy { get; set; }


        public DateTime CreationDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModificationDate { get; set; }
    }
    public class SaveEvaluationSurveySetup
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> EmployeelevelIds { get; set; }
        public List<UserTypeEvaluationSurveySetup> UsertypeData { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int StatusId { get; set; } = 1;
    }
    public class UserTypeEvaluationSurveySetup
    {
        public int UserTypeId { get; set; }
        public int AllowAdd { get; set; }
        public int AllowDelete { get; set; }
        public int AllowRead { get; set; }
        public int AllowUpdate { get; set; }
        public int Order { get; set; }
    }
    public class DeleteEvaluationSurveySetup
    {
        [Required(ErrorMessage = "The Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Id must be bigger than 0")]
        public int Id { get; set; }

        //[Required(ErrorMessage = "The SurveyId is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "The SurveyId must be bigger than 0")]
        //public int SurveyId { get; set; }

    }
}
