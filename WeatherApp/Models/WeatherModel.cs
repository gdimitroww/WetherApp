using System;

namespace WeatherApp.Models
{
    public class WeatherModel
    {
        public string Location { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Condition { get; set; }
        public string Icon { get; set; }
        public string IconClass { get; set; }
        public string ErrorMessage { get; set; }
    }
} 