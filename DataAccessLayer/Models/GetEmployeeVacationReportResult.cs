﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetEmployeeVacationReportResult
    {
        public int EmployeeVacationID { get; set; }
        public int EmployeeID { get; set; }
        public int? VacationTypeID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Notes { get; set; }
        public int? DayCount { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int ProjectID { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public string VacationTypeDesc { get; set; }
        public int EnableDelete { get; set; }
        public string StartDate { get; set; }
        public string companyname { get; set; }
        public string footertitle1 { get; set; }
        public string footertitle2 { get; set; }
        public string imagepath { get; set; }
        public string CurrDate { get; set; }
        public int AttendanceDate { get; set; }
    }
}
