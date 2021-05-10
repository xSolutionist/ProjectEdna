using System;
using Newtonsoft.Json;

namespace ProjectEdnaWeb.Services.WeatherModels {
  [Serializable]
  public class WeatherInfo {
    [JsonProperty ("id")]
    public int Id { get; set; }

    [JsonProperty ("main")]
    public string Main { get; set; }

    [JsonProperty ("description")]
    public string Description { get; set; }

    [JsonProperty ("icon")]
    public string Icon { get; set; }

  }
}