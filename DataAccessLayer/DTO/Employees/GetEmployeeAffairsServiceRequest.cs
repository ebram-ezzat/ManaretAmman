using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class GetEmployeeAffairsServiceRequest:PageModel
    {
        public int? EmployeeHRServiceID { get; set; }
        public int? EmployeeID { get; set; }      
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; }   
        public int? StatusID { get; set; }       
        public int? HRServiceID { get; set; }
    }
    public class GetEmployeeAffairsServiceResponse
    {
        public int? HRServiceID { get; set; }
        public int? EmployeeHRServiceID { get; set; }
        public int? HRServiceDate { get; set; }  
        public string ReasonDesc { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string HRServiceDesc { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? StatusID { get; set; }
        public string StatusDesc { get; set; }
        public int? EnableDelete { get; set; }  // This might be better as a bool if 0/1
        public int? MonthID { get; set; }
        public int? YearID { get; set; }
        public int? BankID { get; set; }
        public int? BranchID { get; set; }
        public string ServiceText { get; set; }
        public string AttachmentDesc { get; set; }
    }

}
