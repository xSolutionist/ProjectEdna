using System;
using Newtonsoft.Json;
namespace ProjectEdnaWeb.Services.WeatherModels{
  [Serializable]
  public class TemperatureReadings{
    [JsonProperty("temp")]
    public double Temperature { get; set; }
    [JsonProperty("feels_like")]
    public double FeelsLike { get; set; }
    [JsonProperty("humidity")]
    public double Humidity { get; set; }
  }
}