using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherApp.OpenWeatherInterface
{

    public enum httpRequestType
    {
        Get, Post, Put, Delete
    }

    public class OpenWeatherMapAPIControl
    {

        private OWMCurrentJSONDataValues.JSONRoot owmCurrentJSONDataValues      = new OWMCurrentJSONDataValues.JSONRoot();
        private OWMForecastJSONDataValues.JSONRoot owmForecastJSONDataValues    = new OWMForecastJSONDataValues.JSONRoot();

        private List<WeatherValues> weatherValuesCurrent    = new List<WeatherValues>();
        private List<WeatherValues> weatherValuesForecast   = new List<WeatherValues>();

        private DateTime dateValue;

        private string openWeatherMapUrl    = string.Empty;
        private string apikey               = string.Empty;
        private string city                 = string.Empty;
        private string zipCode              = string.Empty;


        private string deserialize = string.Empty;

        public httpRequestType HttpMethod { get; set; }

        public string OpenWeatherMapUrl { get => openWeatherMapUrl; set => openWeatherMapUrl = value;   }
        public string Apikey            { get => apikey;            set => apikey = value;              }
        public string City              { get => city;              set => city = value;                }
        public string ZipCode           { get => city;              set => city = value;                }


        public string DeserializeJSON
        {
            get { return deserialize; }

            set
            {
                var jsonResponsTimeInformation = JsonConvert.DeserializeObject(value);

                value = jsonResponsTimeInformation.ToString();

                deserialize = value;
            }
        }


        public OpenWeatherMapAPIControl()
        {
            this.OpenWeatherMapUrl  = string.Empty;
            this.Apikey             = string.Empty;
            this.City               = string.Empty;
            this.HttpMethod         = httpRequestType.Get;
        }

        public OpenWeatherMapAPIControl(string openWeatherMapUrl, string city, string apikey)
        {
            this.OpenWeatherMapUrl  = openWeatherMapUrl;
            this.Apikey             = apikey;
            this.City               = city;
            this.HttpMethod         = httpRequestType.Get;
        }

        public OpenWeatherMapAPIControl(string openWeatherMapUrl, string city, string zipCode, string apikey)
        {
            this.OpenWeatherMapUrl  = openWeatherMapUrl;
            this.Apikey             = apikey;
            this.City               = city;
            this.ZipCode            = zipCode;
            this.HttpMethod         = httpRequestType.Get;
        }


        public List<WeatherValues> GetCurrentRequestValues()
        {
            string url = OpenWeatherMapUrl + "/data/2.5/weather?q=" + City + "&appid=" + Apikey;

            try
            {
                owmCurrentJSONDataValues    = JsonConvert.DeserializeObject<OWMCurrentJSONDataValues.JSONRoot>(GetRequest(url));

                string mainWeatherStatus    = string.Empty;
                string weatherDescription   = string.Empty;

                foreach (var mainWeatherDataValues in owmCurrentJSONDataValues.Weather)
                {
                    mainWeatherStatus       = mainWeatherDataValues.MainWeatherStatus;
                    weatherDescription      = mainWeatherDataValues.WeatherDescription;
                }

                weatherValuesCurrent.Add(

                    new WeatherValues
                    {
                        Date = "",
                        MainWeatherStatus   = mainWeatherStatus,
                        WeatherDescription  = weatherDescription,
                        Temperture          = owmCurrentJSONDataValues.Main.Temp,
                        Humidity            = owmCurrentJSONDataValues.Main.Humidity,
                        WindSpeed           = owmCurrentJSONDataValues.Wind.Speed,
                        CityName            = owmCurrentJSONDataValues.CityName
                    }
                );

                return weatherValuesCurrent;
            }
            catch (Exception e)
            {
                weatherValuesCurrent.Add(

                   new WeatherValues
                   {
                       Date                 = "",
                       MainWeatherStatus    = "Error",
                       WeatherDescription   = "Error",
                       Temperture           = 0,
                       Humidity             = 0,
                       WindSpeed            = 0,
                       CityName             = "Error"
                   }
               );

                return weatherValuesCurrent;
            }         
        }



        public List<WeatherValues> GetForecastRequestValues()
        {
            try
            {
                string urlForecast = "https://api.openweathermap.org" + "/data/2.5/forecast?zip=" + ZipCode + ",de&appid=" + Apikey;

                owmForecastJSONDataValues = JsonConvert.DeserializeObject<OWMForecastJSONDataValues.JSONRoot>(GetRequest(urlForecast));

                short daysCounter = 0;

                foreach (var mainWeatherDataValues in owmForecastJSONDataValues.List)
                {
                    if (Regex.IsMatch(mainWeatherDataValues.Dt_txt, ".*15:00:00.*"))
                    {
                        dateValue = DateTime.Parse(mainWeatherDataValues.Dt_txt, new CultureInfo("en-US"));

                        if (dateValue.Date == DateTime.Today)
                        {
                            //Do nothing
                        }
                        else
                        {
                            daysCounter++;

                            weatherValuesForecast.Add(

                                            new WeatherValues
                                            {

                                                Date                = mainWeatherDataValues.Dt_txt,
                                                Day                 = dateValue.ToString("dddd"),
                                                MainWeatherStatus   = mainWeatherDataValues.Weather[0].MainWeatherStatus,
                                                WeatherDescription  = mainWeatherDataValues.Weather[0].Description,
                                                Temperture          = mainWeatherDataValues.Main.Temp,
                                                Humidity            = mainWeatherDataValues.Main.Humidity,
                                                WindSpeed           = mainWeatherDataValues.Wind.Speed,
                                                CityName            = owmForecastJSONDataValues.City.Name

                                            }
                                    );
                        }
                    }
                }

                if (daysCounter == 4)
                {
                    dateValue = DateTime.Parse(owmForecastJSONDataValues.List[39].Dt_txt, CultureInfo.InvariantCulture);

                    weatherValuesForecast.Add(

                                            new WeatherValues
                                            {
                                                Date                = owmForecastJSONDataValues.List[39].Dt_txt,
                                                Day                 = dateValue.ToString("dddd"),
                                                MainWeatherStatus   = owmForecastJSONDataValues.List[39].Weather[0].MainWeatherStatus,
                                                WeatherDescription  = owmForecastJSONDataValues.List[39].Weather[0].Description,
                                                Temperture          = owmForecastJSONDataValues.List[39].Main.Temp,
                                                Humidity            = owmForecastJSONDataValues.List[39].Main.Humidity,
                                                WindSpeed           = owmForecastJSONDataValues.List[39].Wind.Speed,
                                                CityName            = owmForecastJSONDataValues.City.Name
                                            }
                   );
                }

                return weatherValuesForecast;

            } catch(Exception e)
            {
                weatherValuesForecast.Add( new WeatherValues { Date = "Error", Day = "Error", MainWeatherStatus = "Error", WeatherDescription = "Error", Temperture = 0, Humidity = 0, WindSpeed = 0, CityName = "Error" } );
                weatherValuesForecast.Add( new WeatherValues { Date = "Error", Day = "Error", MainWeatherStatus = "Error", WeatherDescription = "Error", Temperture = 0, Humidity = 0, WindSpeed = 0, CityName = "Error" });
                weatherValuesForecast.Add( new WeatherValues { Date = "Error", Day = "Error", MainWeatherStatus = "Error", WeatherDescription = "Error", Temperture = 0, Humidity = 0, WindSpeed = 0, CityName = "Error" });
                weatherValuesForecast.Add( new WeatherValues { Date = "Error", Day = "Error", MainWeatherStatus = "Error", WeatherDescription = "Error", Temperture = 0, Humidity = 0, WindSpeed = 0, CityName = "Error" });
                weatherValuesForecast.Add( new WeatherValues { Date = "Error", Day = "Error", MainWeatherStatus = "Error", WeatherDescription = "Error", Temperture = 0, Humidity = 0, WindSpeed = 0, CityName = "Error" });

                return weatherValuesForecast;
            }
        }
        public string GetRequest(string url)
        {
            try
            {
                string strResponseValue = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = httpRequestType.Get.ToString();
          
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception("Error code: " + response.StatusCode.ToString());
                    }

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                        }
                    } 
                }

                return strResponseValue;

            } catch(System.Net.WebException e)
            {                
                return e.ToString();
            }           
        }
    }
}

