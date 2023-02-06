using GA4Library.Helpers;
using GA4Library.Interfaces;
using GA4Library.Models;
using Google.Analytics.Data.V1Beta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA4Library.Services
{
    /// <summary>
    /// Google Analytics 4 Service
    /// Nuget:
    /// using Google.Apis.Auth version:1.58.0-beta01
    /// using Google.Api.Gax.Grpc (>=4.1.1 && <5.0.0)
    /// using Grpc.Core(>=2.46.3&&<3.0.0)
    /// </summary>
    public class GA4Serivce : IGoogleAnalyticsService
    {

        #region 建構子      
        public GA4Serivce(string propertyId
            , IList<DateRangeModel> dateRange
            , Google.Protobuf.Collections.RepeatedField<Dimension> dimension
            , Google.Protobuf.Collections.RepeatedField<Metric> metric)
        {
            _propertyId = propertyId;
            _DateRange = dateRange;
            _Dimension = dimension;
            _Metric = metric;
        }
        private GA4Serivce(GA4Serivce other) : this(propertyId: other._propertyId
            , dateRange: other._DateRange
            , dimension: other._Dimension
            , metric: other._Metric)
        { }
        #endregion

        #region 參數設定
        private static string _GA4KeyPath = GAHelper.BaseDirectory + "your account Key.json";
        /// <summary>
        /// GA4-PROPERTY-ID
        /// </summary>
        private string _propertyId { get; set; }
        /// <summary>
        /// Search DateRange
        /// </summary>
        private IList<DateRangeModel> _DateRange { get; set; }
        /// <summary>
        /// Search Dimension
        /// </summary>
        private Google.Protobuf.Collections.RepeatedField<Dimension> _Dimension { get; set; }
        /// <summary>
        /// Search Metric
        /// </summary>
        private Google.Protobuf.Collections.RepeatedField<Metric> _Metric { get; set; }
        /// <summary>
        /// Reports
        /// </summary>
        IList<WebSiteAnalyticsViewModel> _Report { get; set; }
        /// <summary>
        /// Default initialization
        /// </summary>
        public static IGoogleAnalyticsService Default { get; } = new GA4Serivce(propertyId: GoogleParametersPool.PropertyId
            , dateRange: new List<DateRangeModel> { new DateRangeModel { StartDate = DateTime.Now.Date.DateFormat(), EndDate = DateTime.Now.Date.DateFormat() } }
            , dimension: new Google.Protobuf.Collections.RepeatedField<Dimension> { new Dimension { Name = "date" }, }
            , metric: new Google.Protobuf.Collections.RepeatedField<Metric> { new Metric { Name = "screenPageViews" }, new Metric { Name = "totalUsers" }, });

        public IGoogleAnalyticsService WithPropertyId(string propertyId) => propertyId == _propertyId ? this : new GA4Serivce(this) { _propertyId = propertyId };
        public IGoogleAnalyticsService WithDateRange(IList<DateRangeModel> dateRange) => dateRange == _DateRange ? this : new GA4Serivce(this) { _DateRange = dateRange };
        #endregion

        public void RunReport()
        {
            // Using a default constructor instructs the client to use the credentials
            // specified in GOOGLE_APPLICATION_CREDENTIALS environment variable.
            BetaAnalyticsDataClient client = new BetaAnalyticsDataClientBuilder
            {
                CredentialsPath = _GA4KeyPath
            }.Build();
            // Initialize request argument(s)
            RunReportRequest request = new RunReportRequest()
            {
                Property = "properties/" + _propertyId,
                Dimensions = { _Dimension },
                Metrics = { _Metric },
                DateRanges = { _DateRange.ConvertToGA4DateRange() },
            };
            // Make the request
            var response = client.RunReport(request);
        }

        public IList<WebSiteAnalyticsViewModel> GetReport()
        {
            return _Report;
        }

        #region Shared Functions    
        private void BuildReport(RunReportResponse response)
        {
            var rows = response?.Rows;
            if (rows == null || !rows.Any())
            {
                return;
            }
            var createTime = DateTime.Now;
            var result = new List<WebSiteAnalyticsViewModel>();
            var groupDates = rows.GroupBy(x => x.DimensionValues.FirstOrDefault().Value).ToDictionary(x => x.Key, x => x.FirstOrDefault().MetricValues);
            foreach (var date in groupDates)
            {
                var year = int.Parse(date.Key.Substring(0, 4));
                var month = int.Parse(date.Key.Substring(4, 2));
                var day = int.Parse(date.Key.Substring(6, 2));
                var analyticsDate = new DateTime(year, month, day);
                var metricsIndex = _DateRange.GetMetricsIndex(analyticsDate);
                result.Add(new WebSiteAnalyticsViewModel
                {
                    AnalyticsDate = analyticsDate,
                    WebSiteCount = int.TryParse(date.Value[0].Value, out int webSiteCount) ? webSiteCount : 0,
                    IPCount = int.TryParse(date.Value[1].Value, out int ipCount) ? ipCount : 0,
                    CreateTime = createTime,
                });
            }
        }
        #endregion

    }
}
