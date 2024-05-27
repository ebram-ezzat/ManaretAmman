using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveOrUpdateEmployeeEvaluation
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "CategoryName is Required")]
        public string CategoryName { get; set; }

        public int StatusId { get; set; }
    }
}
