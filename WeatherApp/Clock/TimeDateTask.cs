using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Clock
{
    public class TimeDateTask
    {
        public async Task<string> ShowClock()
        {
            DateTime localDate = DateTime.Now;

            string time = string.Empty;

            await Task.Run(() =>
            {
                time = localDate.ToString("T") + " " + localDate.ToString("tt", CultureInfo.InvariantCulture);
            });

            return time;   
        }

        public async Task<string> ShowDayAndDate()
        {
            DateTime localDate = DateTime.Now;

            string dayAndDate = string.Empty;

            await Task.Run(() =>
            {
                dayAndDate = localDate.ToString("dddd, dd MMMM yyyy", new CultureInfo("en-US"));
            });

            return dayAndDate;
        }


    }
}
