﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("NotifciationSetupUserType")]
    public partial class NotifciationSetupUserType
    {
        [Key]
        public int NotifciationSetupID { get; set; }
        [Key]
        public int LevelID { get; set; }
        [Key]
        public int UserTypeID { get; set; }
        [StringLength(2000)]
        [Unicode(false)]
        public string MessageFormat { get; set; }
        [StringLength(2000)]
        [Unicode(false)]
        public string MessageFormatAR { get; set; }
    }
}