using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEvaluationQuestion
    {
        public int? CategoryId { get; set; }


        public string CategoryName { get; set; }

        public int Id { get; set; }

        public string Question { get; set; }
    }
}
