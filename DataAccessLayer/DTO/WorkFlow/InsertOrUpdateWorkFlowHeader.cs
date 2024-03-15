using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.WorkFlow
{
    public class InsertOrUpdateWorkFlowHeader
    {
        public int? WorkflowHeaderID { get; set; } // Output parameter
        public int WorkflowTypeID { get; set; }
        public int TypeID { get; set; }
        public int? CreatedBy { get; set; }       
        public int? ModifiedBy { get; set; } // Nullable int to accommodate NULLs      
        
       
    }
}
