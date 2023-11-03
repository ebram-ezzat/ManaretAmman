﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetEmployeeContractsResult
    {
        public int ContractID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public int? EmployeeID { get; set; }
        public decimal? Salary { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal? SocialSecuritySalary { get; set; }
        public int ProjectID { get; set; }
        public int? IsDailyWork { get; set; }
        public int? ContractTypeID { get; set; }
        public int? ConfirmationDate { get; set; }
        public string StatusDesc { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public string StatusID { get; set; }
        public string ContractTypeDesc { get; set; }
        public int Lastone { get; set; }
        public string ContractTypeValue { get; set; }
    }
}
