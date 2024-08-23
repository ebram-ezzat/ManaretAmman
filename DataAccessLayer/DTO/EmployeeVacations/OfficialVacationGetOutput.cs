using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeVacations
{
    public class OfficialVacationGetOutput
    {
        public int? HolidayID { get; set; }                    
        public int? HolidayTypeID { get; set; }                 
        public int? FromDate { get; set; }                    
        public int? ToDate { get; set; }                        
        public string Notes { get; set; }                      
        public int? CreatedBy { get; set; }                     
        public DateTime? CreationDate { get; set; }             
        public int? ModifiedBy { get; set; }                   
        public DateTime? ModificationDate { get; set; }        
        public int? ProjectID { get; set; }                     
        public string HolidayTypeDesc { get; set; }            
        public int? EnableDelete { get; set; }                  
        public DateTime? v_FromDate { get; set; }              
        public DateTime? v_ToDate { get; set; }                
    }
}
