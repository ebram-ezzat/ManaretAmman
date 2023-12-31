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
    [Table("TestOnlineProject")]
    public partial class TestOnlineProject
    {
        public int ProjectId { get; set; }
        [StringLength(5000)]
        [Unicode(false)]
        public string ProjectName { get; set; }
        public byte[] ProjectLogo { get; set; }
        public int? Statusid { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string TypeDesc { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string validatetype { get; set; }
        [StringLength(1000)]
        [Unicode(false)]
        public string validateid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? validateperidFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? validateperiodto { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Createdby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? creationdate { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string acceptedby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? accepteddate { get; set; }
        public int? OnlineProjectID { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string EMail { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Phone { get; set; }
    }
}