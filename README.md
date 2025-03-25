# Weather Application

A beautiful, responsive web application for checking real-time weather data from around the world built with ASP.NET Core and Docker.

![Weather Application](https://images.unsplash.com/photo-1534088568595-a066f410bcda?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=600&q=80)

## Features

- **Beautiful Weather Interface**: Modern, clean UI with responsive design
- **Global Weather Search**: Get weather for any city worldwide
- **Accurate Location Search**: Support for city,country code format (e.g., "Sofia,BG") for precise results
- **Custom Weather Icons**: Visual weather representations with animations
- **Global Weather View**: Compare weather across major world cities
- **Cross-Platform**: Works on Windows, macOS, and Linux

## Technologies Used

- ASP.NET Core 9.0
- Docker & Docker Compose
- Bootstrap 5
- Font Awesome
- OpenWeatherMap API

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [OpenWeatherMap API Key](https://openweathermap.org/api) (free tier works fine)

### Setup

1. **Clone the repository**
   ```
   git clone https://github.com/yourusername/weather-application.git
   cd weather-application
   ```

2. **Configure API Key**
   
   Create an `appsettings.json` file in the WeatherApp directory with your OpenWeatherMap API key:
   ```json
   {
     "OpenWeatherMapApiKey": "your-api-key-here"
   }
   ```

3. **Build and run with Docker**
   ```
   cd WeatherApp
   docker-compose up --build -d
   ```

4. **Access the application**
   
   Open your browser and navigate to: http://localhost:8080

## Usage

- **Search for Weather**: Enter a city name or "city,country" in the search box
- **Get Precise Results**: Use country codes for better accuracy (e.g., London,UK)
- **View Global Weather**: Click the "View Global Weather" button to see weather for major cities

## Screenshots

- Home Page with Search
- Weather Results Display
- Global Weather View

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Weather data provided by [OpenWeatherMap](https://openweathermap.org/)
- Icons by [Font Awesome](https://fontawesome.com/) 