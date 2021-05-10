using System;
using CheckThat;
using ProjectEdnaWeb;
using ProjectEdnaWeb.Services;
using Xunit;

namespace ProjectEdna.Tests {

  public class WeatherServiceTest {
    [Fact]
    public void check_that_response_is_not_null () {
      var weatherService = new WeatherService ();
      var latitude = "59.333264";
      var longitude = "18.029831";
      var response = weatherService.GetWeatherForCoordinates (latitude, longitude);
      Check.That (() => response != null);
    }

    [Fact]
    public async void check_that_response_is_null_with_wrong_coordinates () {
      var weatherService = new WeatherService ();
      var latitude = "59333264";
      var longitude = "18029831";
      var response = await weatherService.GetWeatherForCoordinates (latitude, longitude);
      Check.That (() => response == null);
    }
  }
}