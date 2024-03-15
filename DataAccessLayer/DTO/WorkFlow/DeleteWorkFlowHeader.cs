using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class DeleteWorkFlowHeader
    {
        [Required(ErrorMessage = "The WorkflowHeaderID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The WorkflowHeaderID must be bigger than 0")]
        public int WorkflowHeaderID { get; set; }
    }
}
