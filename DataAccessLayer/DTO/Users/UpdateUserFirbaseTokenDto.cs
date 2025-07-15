using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Users
{
    public class UpdateUserFirbaseTokenDto
    {
        public int UserId { get; set; }
        public string FirbaseToken { get; set; }
        
    }
}
