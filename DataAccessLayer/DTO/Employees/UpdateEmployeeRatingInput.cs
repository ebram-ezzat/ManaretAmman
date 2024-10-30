using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public  class UpdateEmployeeRatingInput
    {
        public int StatusID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The EvaluationEmployeeID must be bigger than 0")]
        public int EvaluationEmployeeID { get; set; }
    }
}
