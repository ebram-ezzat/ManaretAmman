﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetEmployeeSalaryReport2Result
    {
        public int? YearID { get; set; }
        public int? MonthID { get; set; }
        public decimal? WithSocial { get; set; }
        public decimal? WithoutSocial { get; set; }
        public string companyname { get; set; }
        public string footertitle1 { get; set; }
        public string footertitle2 { get; set; }
        public string imagepath { get; set; }
        public string title { get; set; }
        public string EmployeeName { get; set; }
        public decimal? EmployeeSalary { get; set; }
    }
}
