using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetUserRolesByUserTypeOutput
    {
        public int? userid { get; set; }
        public int? usertypeid { get; set; }
        public int? ischecked { get; set; }
        public string UserName { get; set; }
    }
    public class GetUserRolesByUserTypeInput
    {
        [Required(ErrorMessage = "The UserTypeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The UserTypeID must be bigger than 0")]
        public int UserTypeID { get; set; }
        public int Flag { get; set; } = 1;
    }
}
