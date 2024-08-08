namespace DataAccessLayer.DTO.EmployeeLoans;

public class EmployeeLoanFilter
{
    public int? EmployeeID { get; set; }

    public DateTime? LoanDate { get; set; }

    public int? LoanTypeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? LoanSerial { get; set; }
}

public class EmployeeLoanResult
{
    public int? EmployeeLoanID { get; set; }
    public int? EmployeeID { get; set; }
    public int? LoanDate { get; set; }
    public decimal? LoanAmount { get; set; }
    public string Notes { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModificationDate { get; set; }
    public int? ProjectID { get; set; }
    public string EmployeeName { get; set; }
    public int? EmployeeNumber { get; set; }
    public int? EnableDelete { get; set; }
    public string StartDate { get; set; }
    public string JobTitleName { get; set; }
    public int? Salary { get; set; }
    public string LoanTypeDesc { get; set; }
    public int? LoanTypeID { get; set; }
    public decimal? CurrentValue { get; set; }
    public decimal? ScheduledValue { get; set; }
    public decimal? AllScheduledLoan { get; set; }
    public decimal? MonthlyLoan { get; set; }
    public int? isEnabled { get; set; }
    public int? candelete { get; set; }
    public int? IsPaid { get; set; }
    public string v_loanDate { get; set; }
    public DateTime? vv_Loandate { get; set; }
    public string Acceptstatus { get; set; }
}

public class EmployeeScheduledLoanResult
{
    public int? EmployeeID { get; set; }
    public string EmployeeName { get; set; }
    public int? EmployeeNumber { get; set; }
    public int? LoanDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? RemainAmount { get; set; }
    public string Notes { get; set; }
    public string LoanTypeDesc { get; set; }
    public int? LoanTypeID { get; set; }
    public decimal? AllScheduledLoan { get; set; }
    public decimal? MonthlyLoan { get; set; }
    public int? candelete { get; set; }
    public int? LoanSerial { get; set; }
    public int? EnableDelete { get; set; }
    public string acceptstatus { get; set; }
    public DateTime? v_laonDate { get; set; }
}

public class EmployeeScheduledPaymentsLoanResult
{
    public int? EmployeeLoanID { get; set; }
    public int? EmployeeID { get; set; }
    public int? LoanDate { get; set; }
    public decimal? LoanAmount { get; set; }
    public string Notes { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModificationDate { get; set; }
    public int? ProjectID { get; set; }
    public string EmployeeName { get; set; }
    public int? EmployeeNumber { get; set; }
    public int? EnableDelete { get; set; }
    public string StartDate { get; set; }
    public string JobTitleName { get; set; }
    public int? Salary { get; set; }
    public string LoanTypeDesc { get; set; }
    public int? LoanTypeID { get; set; }
    public decimal? CurrentValue { get; set; }
    public decimal? ScheduledValue { get; set; }
    public decimal? AllScheduledLoan { get; set; }
    public decimal? MonthlyLoan { get; set; }
    public int? isEnabled { get; set; }
    public int? candelete { get; set; }
    public int? IsPaid { get; set; }
    public string v_loanDate { get; set; }
    public DateTime? vv_Loandate { get; set; }
    public string Acceptstatus { get; set; }
    public int? IsFirst { get; set; }
}
