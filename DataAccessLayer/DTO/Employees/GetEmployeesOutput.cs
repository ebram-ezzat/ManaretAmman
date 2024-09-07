using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeesOutput
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? StatusID { get; set; }
        public int? SettingID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string NationalId { get; set; }
        public string SocialNumber { get; set; }
        public string CareerNumber { get; set; }
        public string JobTitleName { get; set; }
        public int? BirthDate { get; set; }
        public int? GenderID { get; set; }
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
        public string EmployeeImage { get; set; } // Assuming image is stored as binary data
        public int ProjectID { get; set; }
        public int? HasBreak { get; set; }
        public int? ContractID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public decimal? Salary { get; set; }
        public decimal? SocialSecuritySalary { get; set; }
        public int? ActualBalance { get; set; }
        public int? PreviousBalance { get; set; }
        public int? CurrentBalance { get; set; }
        public string StatusDesc { get; set; }
        public int? EnableDelete { get; set; }  // Default to true
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
        public int? SSnType { get; set; }
        public int? IsDailyWork { get; set; }
        public int? IsDynamicShift { get; set; }
        public int? ContractTypeID { get; set; }
        public string ContractStatusID { get; set; }
        public int? IncomeTaxType { get; set; } = 1; // Default value 1
        public string EmployeeNameEn { get; set; }
        public int? FamilyCount { get; set; }
        public int? PermitEndDate { get; set; }
        public string PermitNo { get; set; }
        public int? ReligionID { get; set; }
        public int? IsMilitary { get; set; }
        public int? JobTitleID { get; set; }
        public int? SectionID { get; set; }
        public int? CompanyID { get; set; }
        public string EmergencyCallMobile2 { get; set; }
        public string EmergencyCallName2 { get; set; }
        public int? DateForMozawleh { get; set; }
        public int? ParticipateDate { get; set; }
        public int? SSnParticipateDate { get; set; }
        public int? IsViewCompany { get; set; }
        public int? FridayShift { get; set; }
        public int? CShift { get; set; }
        public int? NoOvertime { get; set; }
        public int? StartHealthCertificate { get; set; }
        public int? EndHealthCertificate { get; set; }
        public int? companynameid { get; set; }
        public int? ViewSocialSecurity { get; set; }
        public decimal? Evaluation { get; set; }
        public string Swiftcode { get; set; }
        public int? EmployeeWorkingHours { get; set; }
        public int? EndContract { get; set; }
        public int? EndContractDate { get; set; }
        public int? HasMobileInfo { get; set; }
        public DateTime? v_BirthDate { get; set; } 
        public DateTime? v_StartDate { get; set; }
        public DateTime? v_EndDate { get; set; }

    }
}
