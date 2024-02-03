using DataAccessLayer.DTO.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetUserRolesInput
    {
        [Required(ErrorMessage = "The UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The UserId must be bigger than 0")]
        public int UserId { get; set; }        
        public int Flag { get; set; } = 1;     

    }
    public class GetUserRolesOutput
    {
        public int UserId { get; set; }
        public int IsChecked { get; set; }
        public int UserTypeId { get; set; }

    }

    public class InsertUserRolesInput
    {
        [Required(ErrorMessage = "The UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The UserId must be bigger than 0")]
        public int UserId { get; set;}
        [Required(ErrorMessage = "The UserTypeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The UserTypeId must be bigger than 0")]
        public int UserTypeId { get; set; }
    }
}
