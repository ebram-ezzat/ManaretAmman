﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("Attendance")]
    [Index("DateOnly", Name = "IX_Attendance23")]
    [Index("EmployeeID", Name = "IX_Attendance24")]
    [Index("ProjectID", Name = "IX_Attendance25")]
    [Index("AttDate", Name = "ix_Attendance_2")]
    [Index("ProjectID", "StatusID", Name = "ix_Attendance_3")]
    public partial class Attendance
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AttDate { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Type { get; set; }
        public int? EmployeeID { get; set; }
        public int? StatusID { get; set; }
        public int? attdateint { get; set; }
        [Key]
        public int ProjectID { get; set; }
        public int? DateOnly { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string IP { get; set; }
        public int? Dateasint { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("Attendances")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("Attendances")]
        public virtual Project Project { get; set; }
    }
}