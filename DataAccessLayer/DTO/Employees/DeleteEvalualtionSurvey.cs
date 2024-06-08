using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class DeleteEvalualtionSurvey
    {
        [Required(ErrorMessage = "The Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Id must be bigger than 0")]
        public int Id { get; set; }
    }
}
