﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetEmployeeLoanReportResult
    {
        public int EmployeeID { get; set; }
        public string LoanDate { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public decimal? LoanAmount { get; set; }
        public int? CountPayment { get; set; }
        public decimal? Salary { get; set; }
        public decimal? CurrentValue { get; set; }
        public decimal ScheduledValue { get; set; }
        public string DepartmentDesc { get; set; }
        public string JobtitleName { get; set; }
        public int? StartDate { get; set; }
        public string companyname { get; set; }
        public string footertitle1 { get; set; }
        public string footertitle2 { get; set; }
        public string imagepath { get; set; }
        public string MobileNo { get; set; }
        public string currentDate { get; set; }
        public string NationalId { get; set; }
    }
}
