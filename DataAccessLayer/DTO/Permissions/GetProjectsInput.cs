using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Permissions
{
    public class GetProjectsInput
    {
        public int Flag { get; set; }
        public string Search { get; set; }
    }
    public class GetProjectsOutPutOfFlag2
    {
       public int ProjectID { get; set; }
       public string ProjectName { get; set; }
       public string CompanyName { get; set; }
       public int? IsActive { get; set; }
       public string ReportPath { get; set; }
       public string imagepath {  get; set; }
       public string footertitle1 { get; set; }
       public string footertitle2 { get; set; }
	   public string UserName { get; set; } 
	   public DateTime? validateperidFrom { get; set; }
	   public DateTime? validateperidTo { get; set; }
       public string MAXLogDate { get; set; }
	   public string Phone { get; set; }
    }
}
