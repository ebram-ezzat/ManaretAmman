using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Index("CategoryId", Name = "PK__Evaluati__23CDE590C4A24BAA")]

    public class EvaluationCategory:IMustHaveProject
    {
        [Key]
        public int CategoryId { get; set; }

     
        public string CategoryName { get; set; }

        public int StatusId { get; set; }

        
        public int CreatedBy { get; set; }

       
        public DateTime CreationDate { get; set; } 

        public int? ModifiedBy { get; set; }

        public DateTime? ModificationDate { get; set; }

        [ForeignKey("Projects")]
        public int ProjectID { get; set; }
        public Project Projects { get; set; }
    }
}
