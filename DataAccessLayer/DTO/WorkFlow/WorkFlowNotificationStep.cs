using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class GetWorkFlowNotificationStepInput
    {
        public int? WorkFlowNotificationID { get; set; } = null;
        public int? WorkFlowStepID { get; set; } = null;
        public int? UserTypeID { get; set; } = null;
        //public int? CreatedBy { get; set; } = null;
        //public int? ModifiedBy { get; set; } = null;
        //public DateTime? CreationDate { get; set; } = null;
        //public DateTime? ModificationDate { get; set; } = null;
        public string NotificationDetail { get; set; } = null;
        public string NotificationDetailAr { get; set; } = null;

    }
    public class GetWorkFlowNotificationStepOutput
    {
        public int? WorkFlowNotificationID { get; set; }
        public int? WorkFlowStepID { get; set; }
        public int? UserTypeID { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string NotificationDetail { get; set; }
        public string NotificationDetailAr { get; set; }
        public string UserType { get; set; }
        public string RejectNote { get; set; }
        public string RejectNoteAr { get; set; }

    }
}
