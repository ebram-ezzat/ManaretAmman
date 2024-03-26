using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Lookup
{
    public class GetLookupTableDataInput
    {
        public int? ID { get; set; } = null;
        public int? ProjectID { get; set; } = null;
        public int? ParentID { get; set; } = null;
        [Required(ErrorMessage = "TableName is required")]
        public string TableName { get; set; }
        [Required(ErrorMessage = "ColumnName is required")]
        public string ColumnName { get; set; }         
        //public int? ApprovalPageID { get; set; } = null;
    }
    public class GetLookupTableDataOutput
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; } // Assuming string, adjust based on actual data type
        public string ColumnDescriptionAr { get; set; }
        public string ColumnDescription { get; set; }
        public int? OrderBy { get; set; } // Assuming int, adjust based on actual data type
        public int? Balance { get; set; } // Assuming decimal, adjust based on actual data type
        public string DefaultValue { get; set; } // Assuming string, adjust based on actual data type
        public int? ParentID { get; set; }
        public int? ProjectID { get; set; }
        public int? EnableDelete { get; set; }  // Assuming boolean for enable/disable flags
        public int? EnableUpdate { get; set; }  // Assuming boolean for enable/disable flags
        public string ParentDesc { get; set; }
        public int? WithBank { get; set; } // Nullable bool to accommodate the conditional assignment
        public int? FirstPenaltyPeriod { get; set; }
        public int? SecondPenaltyPeriod { get; set; }
        public int? ThirdPenaltyPeriod { get; set; }
        public int? FourthPenaltyPeriod { get; set; }
        public int? FifthPenaltyPeriod { get; set; }
        public int? FirstPenaltyCategoryID { get; set; }
        public int? PenaltyCategoryID2 { get; set; }
        public int? PenaltyCategoryID3 { get; set; }
        public int? PenaltyCategoryID4 { get; set; }
        public int? PenaltyCategoryID5 { get; set; }
        public decimal WithoutSalaryVacationValue { get; set; } = 0; // Assuming decimal, adjust if different
    }
}
