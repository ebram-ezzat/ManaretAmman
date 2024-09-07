using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveOrUpdateEmployeeAllData
    {
        public SaveOrUpdateEmployeeInFormation SaveOrUpdateEmployeeInFormation { get; set; }
    }
    public class SaveOrUpdateEmployeeInFormation
    {
        public int? EmployeeID { get; set; }
        [Required(ErrorMessage = "The EmployeeName is required.")]       
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        [Required(ErrorMessage = "The StatusID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The StatusID must be bigger than 0")]
        public int? StatusID { get; set; }
        public int? SettingID { get; set; }
        [Required(ErrorMessage = "The NationalId is required.")]

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
        public bool? HasBreak { get; set; } // Assuming boolean
        public DateTime? StartDate { get; set; } // Assuming DateTime
        public int? DepartmentID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? SSNType { get; set; }
        public bool? IsDynamicShift { get; set; } // Assuming boolean
        public string EmployeeNameEn { get; set; }
        public bool? IsMilitary { get; set; } // Assuming boolean
        public int? JobTitleID { get; set; }
        public int? SectionID { get; set; }
        public string EmergencyCallMobile2 { get; set; }
        public string EmergencyCallName2 { get; set; }
        public DateTime? DateForMozawleh { get; set; } // Assuming DateTime
        public int? CompanyNameID { get; set; }

    }
}
