using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using ProjectEdnaWeb.Services.WeatherModels;
namespace ProjectEdnaWeb.Services{
  [Serializable]
  public class WeatherResponse{
    [JsonProperty("weather")]
    public List<WeatherInfo> Weather { get; set; }
    [JsonProperty("main")]
    public TemperatureReadings Temperature { get; set; }
    [JsonProperty("visibility")]
    public double Visibility { get; set; }
  }
}