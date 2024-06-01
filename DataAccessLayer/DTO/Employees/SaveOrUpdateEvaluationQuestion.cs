using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveOrUpdateEvaluationQuestion
    {
        public int Id { get; set; }

        ///<summary>
        ///Get CategoryId as DDL From API Of EvaluationCategory only status 1 (active)
        ///</summary>
        [Range(1,int.MaxValue,ErrorMessage = "CategoryId is must be bigger than 1")]
        [Required(ErrorMessage = "CategoryId is Required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Question is Required")]
        public string Question { get; set; }

       
    }
}
