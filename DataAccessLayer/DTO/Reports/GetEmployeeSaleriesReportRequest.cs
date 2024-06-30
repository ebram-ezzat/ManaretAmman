using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Reports
{
    public class GetEmployeeSaleriesReportRequest:ReportBaseFields
    {
        public int CurrentYearID { get; set; }
        public int CurrentMonthID { get; set; }
        
        public int IsAllEmployees { get; set; }
      
        public int? Withibanonly { get; set; } 
        /// <summary>
        /// <para>. 2 -- بالموجب </para>
        ///<para>.1 -- بالسالب</para>
        /// <para> .الكل :لايتم ارسالها</para>
        /// </summary>
        public int? IsMinus { get; set; }
        /// <summary>
        /// <para>.1 -- للمياومة</para>
        /// <para>.0 -- الموظفين</para>
        /// <para>.null -- الكل</para>
        /// </summary>
        public int? DailyWork { get; set; }
        /// <summary>
        /// Don't Send It
        /// </summary>
        [Obsolete("FromDate is not used ", true)]
        [JsonIgnore]

        public new DateTime? FromDate { get; private set; } = null;
        /// <summary>
        /// Don't Send It
        /// </summary>
        [Obsolete("ToDate is not used ", true)]
        [JsonIgnore]
        public new DateTime? ToDate { get; private set; } = null;
    }
}
