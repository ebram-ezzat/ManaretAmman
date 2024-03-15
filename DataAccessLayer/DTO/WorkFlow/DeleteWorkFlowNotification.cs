using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class DeleteWorkFlowNotification
    {
        [Required(ErrorMessage = "The WorkFlowNotificationID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The WorkFlowNotificationID must be bigger than 0")]
        public int WorkFlowNotificationID { get; set; }
    }
}
