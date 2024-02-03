using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class DeleteUser
    {
        [Required(ErrorMessage = "The UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The UserId must be bigger than 0")]
        public int UserId { get; set; }
    }
}
