﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetScheduleTimeDetailResult
    {
        public int ScheduleTimeID { get; set; }
        public int DayID { get; set; }
        public int DetailID { get; set; }
        public int? FromTime { get; set; }
        public int? totime { get; set; }
        public int? StatusID { get; set; }
    }
}
