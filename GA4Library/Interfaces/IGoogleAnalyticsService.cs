using GA4Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA4Library.Interfaces
{
    /// <summary>
    /// Google Analytics 介面
    /// </summary>
    public interface IGoogleAnalyticsService
    {
        /// <summary>
        /// Set GA4-PROPERTY-ID
        /// </summary>
        /// <param name="propertyId">GA4-PROPERTY-ID</param>
        /// <returns></returns>
        IGoogleAnalyticsService WithPropertyId(string propertyId);
        /// <summary>
        /// Set Search DateRange
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        IGoogleAnalyticsService WithDateRange(IList<DateRangeModel> dateRange);
        /// <summary>
        /// Run Report
        /// </summary>
        void RunReport();
        /// <summary>
        /// Get Report
        /// </summary>
        /// <returns></returns>
        IList<WebSiteAnalyticsViewModel> GetReport();
    }
}
