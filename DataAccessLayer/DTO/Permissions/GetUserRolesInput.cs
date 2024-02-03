using DataAccessLayer.DTO.CustomValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetUserRolesInput
    {
        public int UserId { get; set; }
        public int Falg { get; set; }       
        public int LoginUserId { get; set; }

    }
    public class GetUserRolesOutput
    {
        public int UserId { get; set; }
        public int IsChecked { get; set; }
        public int UserTypeId { get; set; }

    }

    public class InsertUserRolesInput
    {
        public int UserId { get; set;}
        public int UserTypeId { get; set; }
    }
}
