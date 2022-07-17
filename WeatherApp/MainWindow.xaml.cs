using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherApp.Clock;
using WeatherApp.OpenWeatherInterface;

namespace WeatherApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private OpenWeatherMapAPIControl owmacCities;

        private readonly TimeDateTask tdt = new TimeDateTask();
        private readonly double kelvin = Unit.Kelvin;

        public MainWindow()
        {
            InitializeComponent();

            ShowClockOnDisplay();
        }

        private async void ShowClockOnDisplay()
        {
            await Task.Run(() =>
            {
                while (true)
                {

                    string currentTime = tdt.ShowClock().Result;
                    string dayAndDate = tdt.ShowDayAndDate().Result;

                    this.Dispatcher.BeginInvoke(new Action(() => tb_current_clock.Text = currentTime), System.Windows.Threading.DispatcherPriority.Background, null);

                    this.Dispatcher.BeginInvoke(new Action(() => tb_current_date_day.Text = dayAndDate), System.Windows.Threading.DispatcherPriority.Background, null);

                    Task.Delay(1000).Wait();
                }
                
            });
        }


        private async void Btn_search_weather_data_of_city_Click(object sender, RoutedEventArgs e)
        {
            if(tbox_enter_city_name.Text == string.Empty && tbox_enter_zip_code_of_the_city.Text == string.Empty)
            {
                MessageBox.Show("Both text fields are empty, enter a city and the postal code.");

            } else if( tbox_enter_city_name.Text == string.Empty ^ (tbox_enter_zip_code_of_the_city.Text == string.Empty))
            {
                MessageBox.Show("One of the two text fields are empty, please enter a city and the postal code.");
            }
           
            else
            {
                tb_City.Text = tbox_enter_city_name.Text;
                this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/WeatherApp;component/Images/Cities/standard_city.jpg")));

                await Task.Run(() =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() => owmacCities = new OpenWeatherMapAPIControl("https://api.openweathermap.org", tbox_enter_city_name.Text, tbox_enter_zip_code_of_the_city.Text, "41d1e31408dbafee7af530b68bcbe427")), System.Windows.Threading.DispatcherPriority.Background, null);
                    this.Dispatcher.BeginInvoke(new Action(() => CallWeatherCurrentValues(owmacCities.GetCurrentRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
                    this.Dispatcher.BeginInvoke(new Action(() => CallWeatherForecastValues(owmacCities.GetForecastRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);

                });
            }
        }

        private async void Btn_Dresden_Click(object sender, RoutedEventArgs e)
        {
            tb_City.Text = "Dresden";
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/WeatherApp;component/Images/Cities/Dresden.jpg")));            

            await Task.Run(() =>
            {

                owmacCities = new OpenWeatherMapAPIControl("https://api.openweathermap.org", "Dresden", "01069", "41d1e31408dbafee7af530b68bcbe427");
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherCurrentValues(owmacCities.GetCurrentRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherForecastValues(owmacCities.GetForecastRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
            
            });

        }

        private async void Btn_Muenchen_Click(object sender, RoutedEventArgs e)
        {
            tb_City.Text = "Muenchen";
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/WeatherApp;component/Images/Cities/Muenchen.jpg")));

           
            await Task.Run(() =>
            {
                owmacCities = new OpenWeatherMapAPIControl("https://api.openweathermap.org", "Muenchen", "80331", "41d1e31408dbafee7af530b68bcbe427");

                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherCurrentValues(owmacCities.GetCurrentRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherForecastValues(owmacCities.GetForecastRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
            });

        }

        private async void Btn_Heidelberg_Click(object sender, RoutedEventArgs e)
        {
            tb_City.Text = "Heidelberg";
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/WeatherApp;component/Images/Cities/Heidelberg.jpg")));
        
            await Task.Run(() =>
            {
                owmacCities = new OpenWeatherMapAPIControl("https://api.openweathermap.org", "Heidelberg", "69115", "41d1e31408dbafee7af530b68bcbe427");
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherCurrentValues(owmacCities.GetCurrentRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherForecastValues(owmacCities.GetForecastRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
            });

        }

        private async void Btn_Hamburg_Click(object sender, RoutedEventArgs e)
        {
            tb_City.Text = "Hamburg";
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/WeatherApp;component/Images/Cities/Hamburg.jpg")));
          
            await Task.Run(() =>
            {
                owmacCities = new OpenWeatherMapAPIControl("https://api.openweathermap.org", "Hamburg", "20095", "41d1e31408dbafee7af530b68bcbe427");
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherCurrentValues(owmacCities.GetCurrentRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
                this.Dispatcher.BeginInvoke(new Action(() => CallWeatherForecastValues(owmacCities.GetForecastRequestValues())), System.Windows.Threading.DispatcherPriority.Background, null);
            });

        }


        private void CallWeatherCurrentValues(List<WeatherValues> weatherValues)
        {
            tb_celsius_today.Text       = ((int)(weatherValues[0].Temperture + kelvin)).ToString() + " °C";
            tb_description_today.Text   = weatherValues[0].WeatherDescription;

            string imagePath            = ShowImagePath(weatherValues[0].WeatherDescription);
            imageToday.Source           = new BitmapImage(new Uri(@imagePath, UriKind.RelativeOrAbsolute));

            tb_humidity_today.Text      = "Humidity: " + weatherValues[0].Humidity.ToString() + " %";
            tb_windspeed_today.Text     = "Wind speed: " + ((int)(weatherValues[0].WindSpeed * 3.6)).ToString() + " km/h";
        }



        private void CallWeatherForecastValues(List<WeatherValues> weatherValues)
        {
            
            //Day1
            tb_day1.Text                = TranslateWeekdays(weatherValues[0].Day);
            tb_celsius_day1.Text        = ((int)(weatherValues[0].Temperture + kelvin)).ToString() + " °C";
            tb_description_day1.Text    = weatherValues[0].WeatherDescription;

            string imagePath1           = ShowImagePath(weatherValues[0].WeatherDescription);
            imageDay1.Source            = new BitmapImage(new Uri(@imagePath1, UriKind.RelativeOrAbsolute));

            tb_humidity_day1.Text       = "Humidity: "      + weatherValues[0].Humidity.ToString() + " %";
            tb_windspeed_day1.Text      = "Wind speed: "    + ((int)(weatherValues[0].WindSpeed * 3.6)).ToString() + " km/h";


            //Day2
            tb_day2.Text                = TranslateWeekdays(weatherValues[1].Day);
            tb_celsius_day2.Text        = ((int)(weatherValues[1].Temperture + kelvin)).ToString() + " °C";
            tb_description_day2.Text    = weatherValues[1].WeatherDescription;

            string imagePath2           = ShowImagePath(weatherValues[1].WeatherDescription);
            imageDay2.Source            = new BitmapImage(new Uri(@imagePath2, UriKind.RelativeOrAbsolute));

            tb_humidity_day2.Text       = "Humidity: " + weatherValues[1].Humidity.ToString() + " %";
            tb_windspeed_day2.Text      = "Wind speed: " + ((int)(weatherValues[1].WindSpeed * 3.6)).ToString() + " km/h";


            //Day3
            tb_day3.Text                = TranslateWeekdays(weatherValues[2].Day);
            tb_celsius_day3.Text        = ((int)(weatherValues[2].Temperture + kelvin)).ToString() + " °C";
            tb_description_day3.Text    = weatherValues[2].WeatherDescription;

            string imagePath3           = ShowImagePath(weatherValues[2].WeatherDescription);
            imageDay3.Source            = new BitmapImage(new Uri(@imagePath3, UriKind.RelativeOrAbsolute));

            tb_humidity_day3.Text       = "Humidity: " + weatherValues[2].Humidity.ToString() + " %";
            tb_windspeed_day3.Text      = "Wind speed: " + ((int)(weatherValues[2].WindSpeed * 3.6)).ToString() + " km/h";

            //Day4
            tb_day4.Text                = TranslateWeekdays(weatherValues[3].Day);
            tb_celsius_day4.Text        = ((int)(weatherValues[3].Temperture + kelvin)).ToString() + " °C";
            tb_description_day4.Text    = weatherValues[3].WeatherDescription;

            string imagePath4           = ShowImagePath(weatherValues[3].WeatherDescription);
            imageDay4.Source            = new BitmapImage(new Uri(@imagePath4, UriKind.RelativeOrAbsolute));

            tb_humidity_day4.Text       = "Humidity: " + weatherValues[3].Humidity.ToString() + " %";
            tb_windspeed_day4.Text      = "Wind speed: " + ((int)(weatherValues[3].WindSpeed * 3.6)).ToString() + " km/h";


            //Day5
            tb_day5.Text                = TranslateWeekdays(weatherValues[4].Day);
            tb_celsius_day5.Text        = ((int)(weatherValues[4].Temperture + kelvin)).ToString() + " °C";
            tb_description_day5.Text    = weatherValues[4].WeatherDescription;

            string imagePath5           = ShowImagePath(weatherValues[4].WeatherDescription);
            imageDay5.Source            = new BitmapImage(new Uri(@imagePath5, UriKind.RelativeOrAbsolute));

            tb_humidity_day5.Text       = "Humidity: " + weatherValues[4].Humidity.ToString() + " %"; 
            tb_windspeed_day5.Text      = "Wind speed: " + ((int)(weatherValues[4].WindSpeed*3.6)).ToString() + " km/h";

        }

        private string ShowImagePath(string description)
        {
            
            switch (description)
            {

                case "clear sky":

                    return "/Images/Icons/clear_sky.png";
                   

                //Clouds
                case "few clouds":

                    return "/Images/Icons/few_clouds.png";

                case "scattered clouds":

                    return "/Images/Icons/scattered_clouds_2.png";

                case "broken clouds":

                    return "/Images/Icons/broken_clouds.png";

                case "overcast clouds":

                    return "/Images/Icons/overcast_clouds.png";

                //Drizzle
                case "light intensity drizzle":

                    return "/Images/Icons/drizzle.png";

                case "drizzle":

                    return "/Images/Icons/drizzle.png";

                case "heavy intensity drizzle":

                    return "/Images/Icons/drizzle.png";

                case "light intensity drizzle rain":

                    return "/Images/Icons/drizzle.png";

                case "drizzle rain":

                    return "/Images/Icons/drizzle.png";

                case "heavy intensity drizzle rain":

                    return "/Images/Icons/drizzle.png";

                case "shower rain and drizzle":

                    return "/Images/Icons/drizzle.png";

                case "heavy shower rain and drizzle":

                    return "/Images/Icons/drizzle.png";

                case "shower drizzle":

                    return "/Images/Icons/drizzle.png";


                //Rain
                case "shower rain":

                    return "/Images/Icons/shower_rain.png"; 

                case "rain":

                    return "/Images/Icons/rain.png";

                case "light rain":

                    return "/Images/Icons/rain.png";

                case "moderate rain":

                    return "/Images/Icons/rain.png";

                case "heavy intensity rain":

                    return "/Images/Icons/rain.png";

                case "very heavy rain":

                    return "/Images/Icons/rain.png";

                case "extreme rain":

                    return "/Images/Icons/rain.png";

                case "freezing rain":

                    return "/Images/Icons/freezing_rain.png";

                case "light intensity shower rain": 

                    return "/Images/Icons/shower_rain.png";

                case "heavy intensity shower rain":

                    return "/Images/Icons/shower_rain.png";

                case "ragged shower rain":

                    return "/Images/Icons/shower_rain.png";


                //Thunderstorm
                case "thunderstorm":

                    return "/Images/Icons/thunderstorm.png";

                case "thunderstorm with light rain":

                    return "/Images/Icons/thunderstorm.png";

                case "thunderstorm with rain":

                    return "/Images/Icons/thunderstorm.png";

                case "thunderstorm with heavy rain":

                    return "/Images/Icons/thunderstorm.png";

                case "light thunderstorm":

                    return "/Images/Icons/thunderstorm.png";

                case "heavy thunderstorm":

                    return "/Images/Icons/thunderstorm.png";

                case "ragged thunderstorm":

                    return "/Images/Icons/thunderstorm.png";

                case "thunderstorm with light drizzle":

                    return "/Images/Icons/thunderstorm.png";

                case "thunderstorm with drizzle":

                    return "/Images/Icons/thunderstorm.png";

                case "thunderstorm with heavy drizzle":

                    return "/Images/Icons/thunderstorm.png";


                //Snow
                case "light snow":

                    return "/Images/Icons/snow.png";

                case "snow":

                    return "/Images/Icons/snow.png";

                case "Heavy snow":

                    return "/Images/Icons/snow.png";

                case "Sleet":

                    return "/Images/Icons/snow.png";

                case "Light shower sleet":

                    return "/Images/Icons/snow.png";

                case "Shower sleet":

                    return "/Images/Icons/snow.png";

                case "Rain and snow":

                    return "/Images/Icons/freezing_rain.png";

                case "Light shower snow":

                    return "/Images/Icons/snow.png";

                case "Shower snow":

                    return "/Images/Icons/snow.png";

                case "Heavy shower snow":

                    return "/Images/Icons/snow.png";


                //Atmosphere
                case "mist":

                    return "/Images/Icons/mist.png";

                case "Smoke":

                    return "/Images/Icons/smoke.png";

                case "Haze":

                    return "/Images/Icons/mist.png";

                case "sand/ dust whirls":

                    return "/Images/Icons/sand.png";

                case "fog":

                    return "/Images/Icons/mist.png";

                case "sand":

                    return "/Images/Icons/sand.png";

                case "dust":

                    return "/Images/Icons/sand.png";

                case "volcanic ash":

                    return "/Images/Icons/volcanic_ash.png";

                case "squalls":

                    return "/Images/Icons/squalls.png";

                case "tornado":

                    return "/Images/Icons/tornado.png";

                default:

                    return "/Images/Icons/default.png"; ;

            }
        }

        private string TranslateWeekdays(string days)
        {

            switch (days)
            {

                case "Montag":

                    return "Monday";

                case "Dienstag":

                    return "Tuesday";

                case "Mittwoch":

                    return "Wednesday";

                case "Donnerstag":

                    return "Thursday";

                case "Freitag":

                    return "Friday";

                case "Samstag":

                    return "Saturday";

                case "Sonntag":

                    return "Sunday";
                        
                default:

                    return "Error";

            }
        }


    }
}
