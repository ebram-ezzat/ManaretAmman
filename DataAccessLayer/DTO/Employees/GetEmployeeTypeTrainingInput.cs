using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeTypeTrainingInput
    {
        public int? EmployeeID { get; set; }
        public int? ID { get; set; }        
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class GetEmployeeTypeTrainingResponse
    {
        public int? ID { get; set; }
        public int? ProjectID { get; set; }
        public int? TypeID { get; set; }
        public string DocNumber { get; set; }
        public string RevisionNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string ExternalTrainingSubject { get; set; }
        public decimal? ExpectedCost { get; set; }
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
        public int StatusID { get; set; }
        public int ApprovalStatusID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string TrainerName { get; set; }
        public string TrainingTime { get; set; }
        public int? ShowEmployeeComment { get; set; } = 1;
        public int? EnableEmployeeComment { get; set; } = 1;
        public int? ShowHRComment { get; set; } = 1;
        public int? EnableHrComment { get; set; } = 1;
        public int? ShowDirectManagerComment { get; set; } = 1;
        public int? EnableManagerComment { get; set; } = 1;
        public int? ShowTopeManagementComment { get; set; } = 1;
        public int? EnableTopeManagementComment { get; set; } = 1;
    }
}
