var iconElement = document.querySelector(".weather-icon");
var tempElement = document.querySelector(".temperature-value p");
var descElement = document.querySelector(".weather-description p");
var humidElement = document.querySelector(".weather-humidity p");
var windElement = document.querySelector(".weather-windspeed p");
var locationElement = document.querySelector(".location p");

var weatherIcon = null;
var tempurature = null;
var weatherDescription = null;
var humidity = null;
var windSpeed = null;
var weatherCity = null;
var weatherCountry = null;

var KELVIN = 273;
var APIKey = "a4cabe78b524ea8f4ac81279f16adc43";

var longitude = $(".longitude").text().trim();
var latitude = $(".latitude").text().trim();

getWeather(latitude, longitude);

function getWeather(latitude, longitude) {
    let url = `https://api.openweathermap.org/data/2.5/weather?lat=${latitude}&lon=${longitude}&appid=${APIKey}`;
    // get the needed info from the weather api
    fetch(url)
        .then(function (response) {
            let data = response.json();
            return data;
        })
        .then(function (data) {
            weatherIcon = data.weather[0].icon;
            weatherDescription = data.weather[0].description;
            tempurature = Math.floor(data.main.temp - KELVIN);
            console.log("weatherDescription", weatherDescription);
            humidity = data.main.humidity;
            windSpeed = data.wind.speed;          
            weatherCity = data.name;
            weatherCountry = data.sys.country;
        })
        .then(function () {
            displayWeather();
        });
}

// the display of weather on classroom detail page
function displayWeather() {
    iconElement.innerHTML = `<img src="/Image/weatherIcons/${weatherIcon}.png"/>`;
    descElement.innerHTML = `<span>Weather: </span>${weatherDescription}`;
    tempElement.innerHTML = `<span>Temperature: </span>${tempurature}°<span>C</span>`;
    humidElement.innerHTML = `<span>Humidity: </span>${humidity}<span>%</span>`
    windElement.innerHTML = `<span>Wind Speed: </span>${windSpeed}<span> km/h</span>`
    locationElement.innerHTML = `<span>Location: </span>${weatherCity}, ${weatherCountry}`;
}