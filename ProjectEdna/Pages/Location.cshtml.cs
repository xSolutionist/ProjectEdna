using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ProjectEdnaWeb.Services;
using static ProjectEdnaWeb.Services.IpLocationService;

namespace ProjectEdna.Pages {
    public class LocationModel : PageModel {
        private readonly ILogger<LocationModel> _logger;
        private IpLocationService _locationService;
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Country { get; set; }

        public LocationModel (ILogger<LocationModel> logger) {
            _logger = logger;
            _locationService = IpLocationServiceFactory.GetNewLocationService ();
        }

        public void OnGet () {
            var dto = _locationService.GetCurrentLocation ();
            Latitude = dto.Latitud;
            Longitude = dto.Longitud;
            Country = dto.Country;

        }
    }
}