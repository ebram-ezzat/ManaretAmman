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
    [Table("LogError")]
    public partial class LogError
    {
        [StringLength(8000)]
        [Unicode(false)]
        public string errorM { get; set; }
    }
}