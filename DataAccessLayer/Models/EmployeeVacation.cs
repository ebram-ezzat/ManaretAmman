﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeVacation")]
    [Index("VacationTypeID", Name = "IX_EmployeeVactaion_5")]
    [Index("EmployeeID", Name = "ix_Employeevacation_2")]
    [Index("FromDate", Name = "ix_Employeevacation_3")]
    [Index("ToDate", Name = "ix_Employeevacation_4")]
    public partial class EmployeeVacation
    {
        [Key]
        public int EmployeeVacationID { get; set; }
        [Key]
        public int EmployeeID { get; set; }
        public int? VacationTypeID { get; set; }
        public int? FromDate { get; set; }
        public int? ToDate { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string Notes { get; set; }
        public int? DayCount { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Key]
        public int ProjectID { get; set; }
        public int? StatusID { get; set; }
        public int? ApprovalStatusID { get; set; }
        public string imagepath { get; set; }
        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeVacations")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeVacations")]
        public virtual Project Project { get; set; }
    }
}