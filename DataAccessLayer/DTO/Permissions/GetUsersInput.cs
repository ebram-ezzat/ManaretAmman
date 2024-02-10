using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetUsersInput:PageModel
    {
        public int? UserId { get; set; }
        public int? UserTypeId { get; set; }
        public int? Flag { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string BiosID { get; set; }
    }
}
