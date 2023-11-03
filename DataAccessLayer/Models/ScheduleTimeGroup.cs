﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("ScheduleTimeGroup")]
    public partial class ScheduleTimeGroup
    {
        [Key]
        public int ScheduleTimeID { get; set; }
        [Key]
        public int GroupID { get; set; }
        [Key]
        public int ProjectID { get; set; }
        public int? createdby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? creationdate { get; set; }
        public int? modifiedby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? modificationDate { get; set; }
    }
}