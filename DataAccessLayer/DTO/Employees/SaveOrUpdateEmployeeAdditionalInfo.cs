using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveOrUpdateEmployeeAdditionalInfo
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int? EmployeeID { get; set; }
        public int? StatusID { get; set; }
        public int? SettingID { get; set; }
        public int? CreatedBy { get; set; }
        public string NationalId { get; set; }
        public string SocialNumber { get; set; }
        public string CareerNumber { get; set; }
        public string JobTitleName { get; set; }
        public DateTime? BirthDate { get; set; }
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
        public string EmployeeImage { get; set; }
        public int? HasBreak { get; set; }
        public DateTime? StartDate { get; set; }
        public int? DepartmentID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? SSNType { get; set; }
        public int? IsDynamicShift { get; set; }
        public int? Error { get; set; }
        public int? ReligionID { get; set; }
        public int? FamilyCount { get; set; }
        public string PermitNo { get; set; }
        public DateTime? PermitEndDate { get; set; }
        public int? CompanyID { get; set; }
        public int? SectionID { get; set; }
        public DateTime? ParticipateDate { get; set; }
        public DateTime? SSNParticipateDate { get; set; }
        public string EmergencyCallName2 { get; set; }
        public string EmergencyCallMobile2 { get; set; }
        public int? FridayShift { get; set; }
        public int? CShift { get; set; }
        public int? NoOvertime { get; set; }
        public int? EndHealthCertificate { get; set; }
        public int? StartHealthCertificate { get; set; }
        public string Swiftcode { get; set; }
        public int? EmployeeWorkingHours { get; set; }
    }
}
