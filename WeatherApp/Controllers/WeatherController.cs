using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherApp.Models;
using System.Linq;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public WeatherController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            
            // Try to get the API key from environment variable first, then fallback to configuration
            _apiKey = Environment.GetEnvironmentVariable("OpenWeatherMapApiKey") ?? 
                      _configuration["OpenWeatherMapApiKey"] ?? 
                      "YOUR_API_KEY"; // Fallback
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                var emptyModel = new WeatherModel { ErrorMessage = "Please enter a location" };
                return View("Index", emptyModel);
            }

            try
            {
                // Get coordinates
                var coordinates = await GetCoordinates(location);
                if (coordinates.lat == 0 && coordinates.lon == 0)
                {
                    var notFoundModel = new WeatherModel { ErrorMessage = "Location not found" };
                    return View("Index", notFoundModel);
                }

                // Get weather data
                var weatherData = await GetWeatherData(coordinates.lat, coordinates.lon);
                return View("Index", weatherData);
            }
            catch (Exception ex)
            {
                var errorModel = new WeatherModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Index", errorModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllCountries()
        {
            try
            {
                var countries = CountryCapital.GetMajorCountries();
                var weatherList = new List<WeatherModel>();

                foreach (var country in countries)
                {
                    try
                    {
                        var coordinates = await GetCoordinates(country.Capital);
                        if (coordinates.lat != 0 || coordinates.lon != 0)
                        {
                            var weatherData = await GetWeatherData(coordinates.lat, coordinates.lon);
                            weatherList.Add(weatherData);
                        }
                    }
                    catch
                    {
                        // Skip any country that has an error
                        continue;
                    }
                }

                return View(weatherList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error fetching country data: {ex.Message}";
                return View(new List<WeatherModel>());
            }
        }

        private async Task<(double lat, double lon)> GetCoordinates(string location)
        {
            var client = _httpClientFactory.CreateClient();
            
            // Parse the location to check if it contains a country code
            var locationParts = location.Split(',').Select(p => p.Trim()).ToArray();
            string city = locationParts[0];
            string countryCode = locationParts.Length > 1 ? locationParts[1] : "";
            
            // Build the query with limit=5 to get the most relevant results
            string query = !string.IsNullOrEmpty(countryCode) 
                ? $"q={city},{countryCode}&limit=5" 
                : $"q={city}&limit=5";
                
            var response = await client.GetAsync($"http://api.openweathermap.org/geo/1.0/direct?{query}&appid={_apiKey}");
            
            if (!response.IsSuccessStatusCode)
                return (0, 0);

            var content = await response.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<JsonElement[]>(content);
            
            if (locations.Length == 0)
                return (0, 0);
            
            // Try to find the best match
            JsonElement bestMatch = locations[0]; // Default to first result
            
            // If the user entered a specific city name, try to find an exact match first
            if (!string.IsNullOrEmpty(city))
            {
                foreach (var loc in locations)
                {
                    string locName = loc.GetProperty("name").GetString() ?? "";
                    
                    // Check for exact match
                    if (string.Equals(locName, city, StringComparison.OrdinalIgnoreCase))
                    {
                        // If country code is provided, check that too
                        if (!string.IsNullOrEmpty(countryCode))
                        {
                            string locCountry = loc.GetProperty("country").GetString() ?? "";
                            if (string.Equals(locCountry, countryCode, StringComparison.OrdinalIgnoreCase))
                            {
                                bestMatch = loc;
                                break;
                            }
                        }
                        else
                        {
                            bestMatch = loc;
                            break;
                        }
                    }
                }
            }
            
            var lat = bestMatch.GetProperty("lat").GetDouble();
            var lon = bestMatch.GetProperty("lon").GetDouble();

            return (lat, lon);
        }

        private async Task<WeatherModel> GetWeatherData(double lat, double lon)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}");
            
            if (!response.IsSuccessStatusCode)
                return new WeatherModel { ErrorMessage = "Failed to fetch weather data" };

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);
            
            // Extract weather information
            var main = data.GetProperty("main");
            var weatherArray = data.GetProperty("weather");
            var weather = weatherArray[0];
            var location = data.GetProperty("name").GetString();
            var country = data.GetProperty("sys").GetProperty("country").GetString();
            
            // Convert temperature from Kelvin to Celsius
            var tempKelvin = main.GetProperty("temp").GetDouble();
            var tempCelsius = tempKelvin - 273.15;
            
            // Get the weather condition ID and main description
            string weatherCondition = weather.GetProperty("main").GetString();
            int weatherId = weather.GetProperty("id").GetInt32();
            
            // Map weather condition to a Font Awesome icon class
            string iconClass = GetWeatherIconClass(weatherId, weatherCondition);
            
            // Get the state/region if available
            string state = "";
            if (data.TryGetProperty("state", out JsonElement stateElement))
            {
                state = stateElement.GetString() ?? "";
            }
            
            // Format location name with more details
            string formattedLocation = string.IsNullOrEmpty(state) 
                ? $"{location}, {country}" 
                : $"{location}, {state}, {country}";
            
            return new WeatherModel
            {
                Location = formattedLocation,
                City = location ?? "",
                Country = country ?? "",
                Temperature = Math.Round(tempCelsius, 1),
                Humidity = main.GetProperty("humidity").GetInt32(),
                Condition = weather.GetProperty("description").GetString(),
                Icon = $"http://openweathermap.org/img/wn/{weather.GetProperty("icon").GetString()}@2x.png",
                IconClass = iconClass
            };
        }
        
        private string GetWeatherIconClass(int weatherId, string weatherMain)
        {
            // Map weather condition codes to Font Awesome icons
            // Based on OpenWeatherMap condition codes: https://openweathermap.org/weather-conditions
            
            // Thunderstorm
            if (weatherId >= 200 && weatherId < 300)
                return "fas fa-bolt";
                
            // Drizzle or Rain
            if ((weatherId >= 300 && weatherId < 400) || (weatherId >= 500 && weatherId < 600))
                return "fas fa-cloud-rain";
                
            // Snow
            if (weatherId >= 600 && weatherId < 700)
                return "fas fa-snowflake";
                
            // Atmosphere (fog, mist, etc.)
            if (weatherId >= 700 && weatherId < 800)
                return "fas fa-smog";
                
            // Clear
            if (weatherId == 800)
                return "fas fa-sun";
                
            // Clouds
            if (weatherId > 800 && weatherId < 900)
                return "fas fa-cloud";
                
            // Default case
            return "fas fa-cloud";
        }
    }
} 