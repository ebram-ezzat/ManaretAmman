using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class InsertUserRolesByUserType
    {
        [Required]
        public string UserId { get; set; }//string with separted with "; "
        [Required]
        public int UserTypeId { get; set; }
    }
}
