﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    public partial class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DefaultDesc { get; set; }
    }
}