﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetEmployeeHolidayResult
    {
        public int EmployeeID { get; set; }
        public int? DayID { get; set; }
        public int? MonthID { get; set; }
        public int? YearID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
    }
}
