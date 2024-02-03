using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetUserTypeRolesInput
    {
        public int? UserTypeId { get; set; }
        public int Falg { get; set; } = 1;//default 1

    }
    public class GetUserTypeRolesOutput
    {
        public int? UserTypeId { get; set; }
        public int? RoleId { get; set; }
        public int? IsChecked { get; set; }// 0=>Not Checked 1 =>Checked
        public string PageName { get; set; }

    }
}
