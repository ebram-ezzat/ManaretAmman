using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class InsertOrUpdateWorkFlowStep
    {
        public int? WorkFlowStepID { get; set; } // For output parameter handling
        [Required(ErrorMessage = "The WorkflowHeaderID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The WorkflowHeaderID must be bigger than 0")]
        public int WorkflowHeaderID { get; set; }
        public int UserTypeID { get; set; }
        public int CanEdit { get; set; }
        public int CanAdd { get; set; }
        public int CanDelete { get; set; }
        public int AcceptStatusID { get; set; }
        public int RejectStatusID { get; set; }
        public int PreviousRejectStatusID { get; set; }
        public int PreviousAcceptStatusID { get; set; }

        // Optional parameters can be nullable
        //public int? CreatedBy { get; set; }
        //public int? ModifiedBy { get; set; }

    }
}
