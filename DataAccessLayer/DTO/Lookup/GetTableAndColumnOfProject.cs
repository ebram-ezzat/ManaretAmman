using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Lookup
{
    public class GetTableAndColumnOfProject
    {
        [Required(ErrorMessage ="ProjectId is required")]
        [Range(1,int.MaxValue,ErrorMessage ="ProjectId is bigger Than 0")]
        public int ProjectID { get; set; }
    }
}
