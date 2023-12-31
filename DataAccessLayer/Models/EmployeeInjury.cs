﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeInjury")]
    public partial class EmployeeInjury
    {
        [Key]
        public int EmployeeInjuryID { get; set; }
        [Key]
        public int EmployeeID { get; set; }
        public int? InjuryID { get; set; }
        public int? DayCount { get; set; }
        public int? InjuryDate { get; set; }
        [StringLength(4000)]
        [Unicode(false)]
        public string ReasonDesc { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        public int? StatusID { get; set; }
    }
}