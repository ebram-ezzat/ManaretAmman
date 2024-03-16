using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class GetWorkFlowHeaderInput
    {
        public int? WorkflowHeaderID { get; set; } = null;
        public int? WorkflowTypeID { get; set; } = null;
        public int? TypeID { get; set; } = null;
        //public int? CreatedBy { get; set; } = null;
        //public int? ModifiedBy { get; set; } = null;       
        //public DateTime? ModificationDate { get; set; } = null;
        //public DateTime? CreationDate { get; set; } = null;

    }
    public class GetWorkFlowHeaderOutput
    {
        public int? WorkflowHeaderID { get; set; } 
        public int? WorkflowTypeID { get; set; }
        public int? TypeID { get; set; }        
        public int? ProjectID { get; set; }         
        public string HolidayType { get; set; }
        public string WorkFlowType { get; set; }

    }
}
