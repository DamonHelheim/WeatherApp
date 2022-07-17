using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.OpenWeatherInterface
{
    public class WeatherValues
    {
        public string   Date                { get; set; }
        public string   Day                 { get; set; }
        public string   MainWeatherStatus   { get; set; }
        public string   WeatherDescription  { get; set; }

        public double   Temperture          { get; set; }

        public int      Humidity            { get; set; }
        public double   WindSpeed           { get; set; }

        public string   CityName            { get; set; }
    }
}
