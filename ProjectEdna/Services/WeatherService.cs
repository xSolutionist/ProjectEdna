using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectEdnaWeb.DTO;

namespace ProjectEdnaWeb.Services {
  public class WeatherService {
    private HttpClient _client { get; set; }
    private string baseUri = @"http://api.openweathermap.org";
    private string _apiKey = "17fc4848fa665d9b4d51af12aaf6db57";
    public WeatherService () {
      _client = new HttpClient ();
      _client.BaseAddress = new System.Uri (baseUri);
    }

    public async Task<WeatherResponse> GetWeatherForCoordinates (string latitude, string longitude) {

      var response = await _client.GetAsync ($@"data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric").ConfigureAwait (false);
      if (response.StatusCode == System.Net.HttpStatusCode.OK) {
        using (var stream = await response.Content.ReadAsStreamAsync ().ConfigureAwait (false)) {
          using (var jsonReader = new JsonTextReader (new StreamReader (stream))) {
            var serializer = new JsonSerializer ();
            var dto = serializer.Deserialize<WeatherResponse> (jsonReader);
            return dto;
          }
        }
      }
      else
        return null;
    }
  }
}