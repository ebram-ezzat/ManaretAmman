using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO;

public class EmployeeVacationInput
{

    public int ID { get; set; }
    public int EmployeeID { get; set; }
    public int? VacationTypeID { get; set; }
    [Required]
    public DateTime? FromDate { get; set; }
    [Required]
    public DateTime? ToDate { get; set; }
    public string? Notes { get; set; }
    public int? DayCount { get; set; }
    public int? CreatedBy { get; set; }

    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; }

    public DateTime? ModificationDate { get; set; }
    public int ProjectID { get; set; }
    public IFormFile File { get; set; }


}

public class EmployeeVacationResult
{
    public int? EmployeeVacationID { get; set; }
    public int? EmployeeID { get; set; }
    public int? VacationTypeID { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string Notes { get; set; }
    public int? DayCount { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; } 
    public DateTime? ModificationDate { get; set; } 
    public int? ProjectID { get; set; }
    public string EmployeeName { get; set; }
    public int? EmployeeNumber { get; set; }
    public string VacationTypeDesc { get; set; }
    public int? EnableDelete { get; set; }
    public string StatusDesc { get; set; }
    public int? StatusID { get; set; }
    public int? ApprovalStatusID { get; set; }
    public string ApprovalProcessID { get; set; }
    public string ImagePath { get; set; }
}
