using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEvaluationSurvey
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

        public string Notes { get; set; }

        public int StatusId { get; set; }
    }
}
