﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Keyless]
    [Table("EmployeeAttendance20230429")]
    public partial class EmployeeAttendance20230429
    {
        public int EmployeeID { get; set; }
        public int AttendanceDate { get; set; }
        public int? CheckIn { get; set; }
        public int? CheckOut { get; set; }
        public int? WeekDayID { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        public int? SettingID { get; set; }
        public int? BreakIn { get; set; }
        public int? BreakOut { get; set; }
        public int ProjectID { get; set; }
        public int? TempCheckIn { get; set; }
        public int? TempCheckOut { get; set; }
        public int? TempCheckinStatusID { get; set; }
        public int? TempCheckoutStatusID { get; set; }
        public int? ISAddedManual { get; set; }
    }
}