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
    public class EvaluationQuestion:IMustHaveProject
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("EvaluationCategory")]
        public int CategoryId { get; set; }
      
        public string Question { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModificationDate { get; set; }
        [ForeignKey("Project")]
        public int ProjectID { get; set ; }
       
        public  Project Project { get; set; }


        public EvaluationCategory EvaluationCategory { get; set; } 

    }
}
