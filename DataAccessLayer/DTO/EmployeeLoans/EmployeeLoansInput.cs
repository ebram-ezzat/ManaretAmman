﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO
{
    public class EmployeeLoansInput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        [Required]
        public DateTime? LoanDate { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        [Required]
        public decimal? LoanAmount { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }
        public int? LoantypeId { get; set; }

        public int? LoanSerial { get; set; }
        public int? IsPaid { get; set; } = 0;
        public int? IsFirst { get; set; }
        public int? EmployeeLoanID { get; set; }
    }
    public class SchededuledLoansInput
    {
        public int TotalAmount { get; set; }
        public int EmployeeID { get; set; }
        public List<EmployeeLoansInput> EmployeeLoansInputs { get; set; }
    }
}
