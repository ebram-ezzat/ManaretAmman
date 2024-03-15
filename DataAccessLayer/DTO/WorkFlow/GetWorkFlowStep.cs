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
        public DateTime? ModificationDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
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
        public DateTime? ModificationDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
