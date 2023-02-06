using GA4Library.Helpers;
using GA4Library.Models;
using GA4Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA4Library
{
    public class TestProgram
    {
        static void Main(string[] args)
        {          
            //set GA Service parameter
            var date = DateTime.Now.AddDays(-1).Date;
            var dateRanges = new List<DateRangeModel>() { new DateRangeModel { StartDate = date.DateFormat(), EndDate = date.DateFormat() } };
            var _gaService = GA4Serivce.Default.WithDateRange(dateRanges);
            _gaService.RunReport();
            //Get Report
            var report = _gaService.GetReport();
        }
    }    
}
