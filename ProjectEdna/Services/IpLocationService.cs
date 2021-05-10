using System;
using System.Collections.Generic;
using System.Net;
using IPGeolocation;
using ProjectEdnaWeb.DTO;

namespace ProjectEdnaWeb.Services {
  public class IpLocationService {
    private IPGeolocationAPI API;
    private WebClient Client;
    private IpLocationService (string apiKey) {
      API = new IPGeolocationAPI (apiKey);
      Client = new WebClient ();

    }
    public LocationDTO GetLocationInfo (string ip) {
      var parameters = new GeolocationParams ();
      parameters.SetIPAddress (ip);
      parameters.SetFields ("geo,time_zone,currency");
      Geolocation response;
      try {
        response = API.GetGeolocation (parameters);
        if (response.GetStatus () == 200)
          return new LocationDTO (response.GetLongitude (), response.GetLatitude (), response.GetCountryName ());
      } catch {
        return null;
      }
      return null;
    }

    public LocationDTO GetCurrentLocation () => GetLocationInfo (Client.DownloadString ("http://icanhazip.com"));

    public static class IpLocationServiceFactory {
      public static IpLocationService GetNewLocationService () {
        return new IpLocationService ("2fa95dc92ec7472bb615638aa6789d15");
      }
    }
  }
}