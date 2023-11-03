﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeExperience")]
    public partial class EmployeeExperience
    {
        [Key]
        public int EmployeeID { get; set; }
        [Key]
        public int DetailID { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Position1 { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string CompanyName { get; set; }
        public int? CountryID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string WorkDescription { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Key]
        public int ProjectID { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeExperiences")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeExperiences")]
        public virtual Project Project { get; set; }
    }
}