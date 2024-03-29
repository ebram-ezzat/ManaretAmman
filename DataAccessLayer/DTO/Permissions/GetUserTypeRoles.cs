﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetUserTypeRolesInput
    {
        [Required(ErrorMessage ="UserTypeId is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The UserTypeId must be bigger than 0")]
        public int? UserTypeId { get; set; }
        public int Flag { get; set; } = 1;//default 1
        public int? CurrentProjectID { get; set; }

    }
    public class GetUserTypeRolesOutput
    {
        public int? UserTypeId { get; set; }
        public int? RoleId { get; set; }
        public int? IsChecked { get; set; }// 0=>Not Checked 1 =>Checked
        public string PageName { get; set; }
        public int? AllowEdit { get; set; }
        public int? AllowDelete { get; set; }
        public int? AllowAdd { get; set; }

    }
}
