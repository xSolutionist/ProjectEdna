using System;
using CheckThat;
using ProjectEdnaWeb;
using ProjectEdnaWeb.Services;
using Xunit;

namespace ProjectEdna.Tests {
    public class IpLocationServiceTest {
        [Fact]
        public void can_get_location_given_arbitrary_ip () {
            const string ip = "195.74.38.137"; //iths.se
            var locationService = IpLocationService.IpLocationServiceFactory.GetNewLocationService ();
            var location = locationService.GetLocationInfo (ip);
            Check.That (() => location.Country == "Sweden");
        }

        [Fact]
        public void can_get_current_location () {
            var locationService = IpLocationService.IpLocationServiceFactory.GetNewLocationService ();
            var location = locationService.GetCurrentLocation ();
            Check.That (() => location.Country == "Sweden");
        }

        [Fact]
        public void get_null_location_for_wrong_ip () {
            var locationService = IpLocationService.IpLocationServiceFactory.GetNewLocationService ();
            var location = locationService.GetLocationInfo ("300.0.0.1");
            Check.That (() => location == null);
        }
    }
}