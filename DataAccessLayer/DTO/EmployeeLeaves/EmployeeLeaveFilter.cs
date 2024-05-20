namespace DataAccessLayer.DTO.EmployeeLeaves;

public class EmployeeLeaveFilter
{
    public int? EmployeeID { get; set; }

    public int? LeaveTypeID { get; set; }

    public DateTime? LeaveDate { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public string FromTime { get; set; } = string.Empty;

    public string ToTime { get; set; } = string.Empty;
    public string imagepath { get; set; }
}
public class EmployeeLeaveResult
{
    public int? EmployeeLeaveID { get; set; }
    public int? EmployeeID { get; set; }
    public int? LeaveTypeID { get; set; }
    public int? LeaveDate { get; set; }
    public int? FromTime { get; set; } 
    public int? ToTime { get; set; } 
    public int? CreatedBy { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModificationDate { get; set; } 
    public int? BySystem { get; set; }
    public int? ProjectID { get; set; } 

    public int? EmployeeNumber { get; set; }
    public string EmployeeName { get; set; }
    public string LeaveTypeDesc { get; set; }
    public int? EnableDelete { get; set; }
    public int? StatusID { get; set; }
    public int? ApprovalStatusID { get; set; }
    public string StatusDesc { get; set; }
    public string FromTimeAsString { get; set; } 
    public string ToTimeAsString { get; set; } 
    public string ImagePath { get; set; }
    public DateTime? v_leaveDate { get; set; }
}
