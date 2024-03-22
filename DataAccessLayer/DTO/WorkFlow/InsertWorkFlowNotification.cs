using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class InsertWorkFlowNotification
    {
        [Required(ErrorMessage = "NotificationSetupID is required")]
        [Range(1, int.MaxValue,ErrorMessage = "NotificationSetupID is bigger than 0")]
        public int NotificationSetupID { get; set; }

        [Required(ErrorMessage = "UserTypeID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserTypeID is bigger than 0")]
        public int UserTypeID { get; set; }
        [Required(ErrorMessage = "MessageFormatAr is required")]
        public string MessageFormatAr { get; set; }
        [Required(ErrorMessage = "MessageFormatEn is required")]
        public string MessageFormatEn { get; set; } 
        public int? IsSMS { get; set; } = null;
        public int? IsEmail { get; set; } = null;
        public int? IsWhatsapp { get; set; } = null;
        public int? IsSystem { get; set; } = null;
    }
}
