﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("SalaryExtraMonth")]
    public partial class SalaryExtraMonth
    {
        [Key]
        public int MonthID { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SalaryPercentage { get; set; }
        [Key]
        public int ProjectID { get; set; }
    }
}