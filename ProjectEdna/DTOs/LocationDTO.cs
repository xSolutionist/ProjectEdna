using System;

namespace ProjectEdnaWeb.DTO {
  public class LocationDTO {
    public LocationDTO (string longitud, string latitud, string country) {
      Longitud = longitud;
      Latitud = latitud;
      Country = country;
    }
    public string Longitud { get; set; }
    public string Latitud { get; set; }
    public string Country { get; set; }
    public override string ToString () {
      return Longitud + "," + Latitud + "," + Country;
    }
  }
}