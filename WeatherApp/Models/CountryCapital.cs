namespace WeatherApp.Models
{
    public class CountryCapital
    {
        public string Country { get; set; }
        public string Capital { get; set; }

        // This list contains a selection of countries and their capitals
        public static List<CountryCapital> GetMajorCountries()
        {
            return new List<CountryCapital>
            {
                new CountryCapital { Country = "United States", Capital = "Washington" },
                new CountryCapital { Country = "United Kingdom", Capital = "London" },
                new CountryCapital { Country = "France", Capital = "Paris" },
                new CountryCapital { Country = "Germany", Capital = "Berlin" },
                new CountryCapital { Country = "Japan", Capital = "Tokyo" },
                new CountryCapital { Country = "China", Capital = "Beijing" },
                new CountryCapital { Country = "Russia", Capital = "Moscow" },
                new CountryCapital { Country = "Canada", Capital = "Ottawa" },
                new CountryCapital { Country = "Australia", Capital = "Canberra" },
                new CountryCapital { Country = "Brazil", Capital = "Brasilia" },
                new CountryCapital { Country = "India", Capital = "New Delhi" },
                new CountryCapital { Country = "South Africa", Capital = "Pretoria" },
                new CountryCapital { Country = "Italy", Capital = "Rome" },
                new CountryCapital { Country = "Spain", Capital = "Madrid" },
                new CountryCapital { Country = "Mexico", Capital = "Mexico City" }
            };
        }
    }
} 