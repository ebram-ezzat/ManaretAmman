using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class GetWorkFlowStepInput
    {
        public int? WorkFlowStepID { get; set; }
        public int? WorkflowHeaderID { get; set; }
        public int? UserTypeID { get; set; }
        public int? CanEdit { get; set; }
        public int? CanAdd { get; set; }
        public int? CanDelete { get; set; }
        public int? AcceptStatusID { get; set; }
        public int? RejectStatusID { get; set; }
       
    }
    public class GetWorkFlowStepOutput
    {
        public int? WorkFlowStepID { get; set; }
        public int? WorkflowHeaderID { get; set; }
        public int? UserTypeID { get; set; }
        public int? CanEdit { get; set; }
        public int? CanAdd { get; set; }
        public int? CanDelete { get; set; }
        public int? AcceptStatusID { get; set; }
        public int? RejectStatusID { get; set; }
        public string UserType { get; set; }
        public string AcceptStatus { get; set; }
        public string RejectStatus { get; set; }
        public int? PreviousRejectStatusID { get; set; }
        public int? PreviousAcceptStatusID { get; set; }
        public string PreviousRejectStatusDesc { get; set; }
        public string PreviousAcceptStatusDesc { get; set; }

    }
}
