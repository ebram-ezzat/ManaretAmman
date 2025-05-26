using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class InsertEmployeeTypeTraining
    {
        public int? ID { get; set; }        
        public int? TypeID { get; set; }
        public string DocNumber { get; set; }
        public string RevisionNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string ExternalTrainingSubject { get; set; }
        public string ExpectedCost { get; set; }
        public string JustificationForRequest { get; set; }
        public string DepartmentHeadComments { get; set; }
        public string HRComments { get; set; }
        public string TopManagementComments { get; set; }
        public string RejectReason { get; set; }
        public string Notes { get; set; }
        public string TrainingLocation { get; set; }
        public string TrainingPeriod { get; set; }
        public string TrainingCompany { get; set; }
        public string TrainingObjective { get; set; }
        public int? StatusID { get; set; }
        public int? ApprovalStatusID { get; set; }
        public int? CreatedBy { get; set; }
        public string TrainerName { get; set; }
        public int? TrainingTime { get; set; }
    }
}
