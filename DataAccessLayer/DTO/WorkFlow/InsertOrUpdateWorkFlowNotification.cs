using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class InsertOrUpdateWorkFlowNotification
    {
        public int? WorkFlowNotificationID { get; set; }
        [Required(ErrorMessage = "The WorkFlowStepID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The WorkFlowStepID must be bigger than 0")]
        public int WorkFlowStepID { get; set; }

        public int UserTypeID { get; set; }

        public string NotificationDetail { get; set; }
        public string NotificationDetailAr { get; set; }


    }
}
