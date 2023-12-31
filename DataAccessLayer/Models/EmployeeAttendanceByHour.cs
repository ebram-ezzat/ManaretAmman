﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    public partial class EmployeeAttendanceByHour
    {
        [Key]
        public int EmployeeID { get; set; }
        [Key]
        public int AttendanceDate { get; set; }
        public int? ShiftID1 { get; set; }
        public int? ShiftID2 { get; set; }
        public int? ShiftID3 { get; set; }
        public int? WorkingHours1 { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Creationdate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        public int? IsWorkigDay { get; set; }
        public int? WorkingHours2 { get; set; }
        public int? WorkingHours3 { get; set; }
    }
}