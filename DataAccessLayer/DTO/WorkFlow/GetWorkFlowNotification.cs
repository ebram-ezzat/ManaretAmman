using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class GetWorkFlowNotificationInput
    {
        [Required(ErrorMessage = "NotificationSetupID is Required")]
        //[Range(1, int.MaxValue,ErrorMessage = "NotificationSetupID Should be bigger than 0" )]
        public int NotificationSetupID { get; set; }
        public int UserTypeID { get; set; }

        /// <summary>
        /// Default is 1
        /// </summary>
        public int Flag { get; set; } = 1;
        /// <summary>
        /// Default is 1 
        /// </summary>
        public int LanguageID { get; set; } = 1;
    }
    public class GetWorkFlowNotificationOutput
    {
        public int NotificationSetupID { get; set; }
        public int UserTypeID { get; set; }

        /// <summary>
        /// This is UserType Description
        /// </summary>
        public string ColumnDescription { get; set; } 
        public string MessageFormatAr { get; set; }
        public string MessageFormatEn { get; set; }
        public int? IsSMS { get; set; } 
        public int? IsEmail { get; set; } 
        public int? IsWhatsapp { get; set; } 
        public int? IsSystem { get; set; } 
        public int? CreatedBy { get; set; } 
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; } 
    }
}
