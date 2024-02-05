using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class InsertUserTypeRoles
    {
        public int UserTypeId { get; set; }
        public string RoleId { get; set; } //string with separted with "; "
        public string AllowEdit { get; set; }//string with separted with "; "
        public string AllowDelete {  get; set; }//string with separted with ";"
        public string AllowAdd { get; set; }//string with separted with ";"
    }
}
