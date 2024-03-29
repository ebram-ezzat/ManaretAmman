﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class EmployeeProfile
    {
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? StatusID { get; set; }
        public string NationalId { get; set; }
        public string SocialNumber { get; set; }
        public string CareerNumber { get; set; }
        public string JobTitleName { get; set; }
        public string BirthDate { get; set; }
        public int? NationalityID { get; set; }
        public string MobileNo { get; set; }
        public string EmailNo { get; set; }
        public int? MaritalStatusID { get; set; }
        public string EmergencyCallName { get; set; }
        public string EmergencyCallMobile { get; set; }
        public string AccountNo { get; set; }
        public string IBAN { get; set; }
        public int? BankID { get; set; }
        public int? BranchID { get; set; }
        public int? ProjectID { get; set; }
        public string companyname { get; set; }
        public string footertitle1 { get; set; }
        public string footertitle2 { get; set; }
        public string imagepath { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal? Salary { get; set; }
        public decimal? SocialSecuritySalary { get; set; }
        public int? ActualBalance { get; set; }
        public int? PreviousBalance { get; set; }
        public int? CurrentBalance { get; set; }
        public string StatusDesc { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public int? SSnType { get; set; }
        public string EmployeeAddress { get; set; }
        public string NationalityDesc { get; set; }
        public string ContractPeriod { get; set; }
        public string SalarySlipDesc { get; set; }
        public string ReliogionDesc { get; set; }
        public string MaritalStatusDesc { get; set; }
        public decimal? TAX { get; set; }
        public int? FamilyCount { get; set; }
        public string PermitNo { get; set; }
        public string PermitEndDate { get; set; }
        public string HrManager { get; set; }
        public string MainBankName { get; set; }
        public string CurrentDate { get; set; }
        public string companynationalid { get; set; }
        //public string todayDate { get; set; }
        public string EmployeeImage { get; set; }
        public decimal? Evaluation { get; set; }        
    }
    public class EmplyeeProfileVModel: EmployeeProfile
    {
        public object ImgBase64 { get; set; }
    }
}
