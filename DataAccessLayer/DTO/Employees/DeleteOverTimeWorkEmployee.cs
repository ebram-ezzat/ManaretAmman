﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class DeleteOverTimeWorkEmployee
    {
        [Required(ErrorMessage = "The EmployeeApprovalID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeApprovalID must be bigger than 0")]
        public int EmployeeApprovalID { get; set; }

    }
}
