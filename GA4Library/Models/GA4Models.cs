using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA4Library.Models
{
    /// <summary>
    /// 時間區間-模型
    /// </summary>
    public class DateRangeModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class WebSiteAnalyticsViewModel
    {
        public int SeqNo { get; set; }
        public DateTime AnalyticsDate { get; set; }
        public int IPCount { get; set; }
        public int WebSiteCount { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
