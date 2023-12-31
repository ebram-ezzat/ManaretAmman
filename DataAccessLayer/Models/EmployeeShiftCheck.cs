﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeShiftCheck")]
    public partial class EmployeeShiftCheck
    {
        [Key]
        public int EmployeeID { get; set; }
        [Key]
        public int ShiftID { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeShiftChecks")]
        public virtual Employee Employee { get; set; }
    }
}