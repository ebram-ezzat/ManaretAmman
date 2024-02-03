using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class InsertUser
    {
      public int? UserId { get; set; }
      public string UserName { get; set; }
      public string UserPassword { get; set; }
      public int? FromOtherProcedure { get; set; }
      public int? StatusID { get; set; }
      public int? UserTypeID { get; set; }
    }
}
