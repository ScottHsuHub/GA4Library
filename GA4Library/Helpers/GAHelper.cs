using GA4Library.Models;
using Google.Analytics.Data.V1Beta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA4Library.Helpers
{
    public static class GAHelper
    {
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// Date Format 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        public static DateRange ConvertToGA4DateRange(this DateRangeModel dateRange)
        {
            if (dateRange == null)
            {
                return default;
            }
            return new DateRange() { StartDate = dateRange.StartDate, EndDate = dateRange.EndDate };
        }
        public static Google.Protobuf.Collections.RepeatedField<DateRange> ConvertToGA4DateRange(this IList<DateRangeModel> dateRange)
        {
            if (dateRange == null)
            {
                return default;
            }
            var result = new Google.Protobuf.Collections.RepeatedField<DateRange>();
            foreach (var item in dateRange)
            {
                result.Add(item.ConvertToGA4DateRange());
            }
            return result;
        }
        /// <summary>
        /// Get Metrics Index
        /// </summary>
        /// <param name="dateRanges"></param>
        /// <param name="analyticsDate"></param>
        /// <returns></returns>
        public static int GetMetricsIndex(this IList<DateRangeModel> dateRanges, DateTime analyticsDate)
        {
            if (dateRanges == null || !dateRanges.Any())
            {
                return default;
            }
            for (var i = 0; i < dateRanges.Count; i++)
            {
                var dateRange = dateRanges[i];
                var st = dateRange.StartDate.ToDateTime();
                var et = dateRange.EndDate.ToDateTime();
                if (analyticsDate >= st && analyticsDate <= et)
                {
                    return i;
                }
            }
            return default;
        }
        /// <summary>
        /// string to datetime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string str)
        {
            if (DateTime.TryParse(str, out var result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
