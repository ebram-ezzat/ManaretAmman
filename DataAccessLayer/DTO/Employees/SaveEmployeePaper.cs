using DataAccessLayer.DTO.CustomValidations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Employees
{
    public class SaveEmployeePaper
    {
        [Required(ErrorMessage = "The EmployeeID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The EmployeeID must be bigger than 0")]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "The PaperID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The PaperID must be bigger than 0")]
        public int PaperID { get; set; }
        
        public string Notes { get; set; }
        [CustomValidationLoginUserID]
        public int CreatedBy { get; set; }
        //GetAttachfromsetting
        // genertedName
        //Save DataBase :GetAttachfromsetting+ConcatTo genertedName+extentsion
        //get file and save to dictory Info 
        public IFormFile File { get; set; }

    }
}
