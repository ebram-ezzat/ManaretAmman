﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("AttendanceAutomaticAction")]
    public partial class AttendanceAutomaticAction
    {
        [Key]
        public int ProjectID { get; set; }
        public int? EmployeeID { get; set; }
        public int? AttendanceDate { get; set; }
        public int? OldCheckIn { get; set; }
        public int? OldCheckout { get; set; }
        public int? OldBreakIn { get; set; }
        public int? OldBreakOut { get; set; }
        public int? ActionTypeID { get; set; }
        public int? OldEmployeeLeaveID { get; set; }
        public int? OldEmployeeVacationID { get; set; }
        public int? NewEmployeeLeaveID { get; set; }
        public int? NewEmployeeVacationID { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Key]
        public int AttendanceAutomaticActionID { get; set; }
        public int? StatusID { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("AttendanceAutomaticActions")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("AttendanceAutomaticActions")]
        public virtual Project Project { get; set; }
    }
}