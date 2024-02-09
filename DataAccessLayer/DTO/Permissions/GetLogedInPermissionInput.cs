using DataAccessLayer.DTO.CustomValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetLogedInPermissionInput
    {
        /// <summary>
        /// Act as loggedin user id 
        /// 
        /// </summary>
        [CustomValidationLoginUserID]
        
        public int UserId { get; set; }
        /// <summary>
        /// Flag Here Should be 3
        /// </summary>
        public int Flag {  get; set; }
    }
    public class GetLogedInPermissionOutput
    {
        public int? RoleID { get; set; }
        public int? AllowEdit { get; set; }
        public int? AllowDelete { get; set; }
        public int? AllowAdd { get; set; }
        public int? DefaultValue { get; set; }


    }
}
