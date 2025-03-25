# Weather Application

This is a web application that allows users to check the current weather for any city or country. It also provides a feature to view weather information for capital cities around the world.

## Prerequisites

- .NET 6.0 SDK or later
- An API key from OpenWeatherMap (https://openweathermap.org/api)
- Docker (optional, for containerized deployment)

## Setting up the API Key

There are three ways to provide your OpenWeatherMap API key:

1. **Environment Variable**: Set an environment variable named `OpenWeatherMapApiKey`
2. **Configuration File**: Edit `appsettings.json` and replace "YOUR_API_KEY" with your actual key
3. **Docker Environment**: Set it in the docker-compose.yml file

## Running the Application

### Using Visual Studio
1. Open the solution in Visual Studio
2. Press F5 to run the application

### Using .NET CLI
1. Open a terminal
2. Navigate to the project directory
3. Run the following command:
   ```
   dotnet run
   ```
4. Open a browser and navigate to `https://localhost:7000` or `http://localhost:5000`

### Using Docker (Recommended for macOS)
1. Make sure Docker Desktop is installed and running on your Mac
2. Open a terminal and navigate to the project directory
3. Set your API key in the docker-compose.yml file:
   ```yaml
   environment:
     - OpenWeatherMapApiKey=YOUR_ACTUAL_API_KEY
   ```
4. Build and run the Docker container:
   ```
   docker-compose up --build
   ```
5. Access the application at `http://localhost:8080` or `https://localhost:8443`

## Features

### Weather Search
- Enter a city or country name to get the current weather
- View temperature, humidity, and weather conditions
- See a visual representation of the current weather

### All Countries
- View weather information for major capital cities around the world
- Compare weather conditions across different regions

## Cross-Platform Support

This application is built with ASP.NET Core, which supports cross-platform development and can run on:
- Windows
- macOS (native or Docker)
- Linux

## Technologies Used

- ASP.NET Core MVC
- Bootstrap for responsive UI
- OpenWeatherMap API for weather data
- Docker for containerization 