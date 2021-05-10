using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectEdna.Data;
using ProjectEdna.Model;
using ProjectEdnaWeb.DTO;
using ProjectEdnaWeb.Services;

namespace ProjectEdna.Pages {
  public class SampleDataModel : PageModel {

    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public SampleDataModel (ApplicationDbContext context,
      UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)

    {
      _context = context;

      _signInManager = signInManager;
      _userManager = userManager;

    }

    [BindProperty]
    public ImageModel ImageModel { get; set; }

    [BindProperty]
    public string ImageFileName { get; set; }

    [BindProperty]
    public string QRFileName { get; set; }

    [BindProperty (SupportsGet = true)]
    public string UserId { get; set; }

    public bool DataExtracted { get; set; }

    [BindProperty (SupportsGet = true)]
    public DateTime GPSDate { get; set; }

    [BindProperty (SupportsGet = true)]
    [Display (Name = "Kordinat Latitude")]
    public double? GPSLatitude { get; set; }

    [BindProperty (SupportsGet = true)]
    [Display (Name = "Kordinat Longitude")]
    public double? GPSLongitude { get; set; }

    [BindProperty (SupportsGet = true)]
    public double? GPSAltitude { get; set; }

    public async Task<IActionResult> OnGetAsync (int? id) {
      if (id == null) {
        return NotFound ();
      }

      ImageModel = await _context.ImageData.FirstOrDefaultAsync (m => m.ID == id);

      if (ImageModel == null) {
        return NotFound ();
      }

      QRFileName = ImageModel.QRCode;
      ImageFileName = ImageModel.Image;
      GPSLatitude = ImageModel.Latitude;
      GPSLongitude = ImageModel.Longitude;
      var user = await _userManager.GetUserAsync (User);
      ImageModel.UserId = user.Id;

      WeatherService weather = new WeatherService ();
      WeatherResponse response = new WeatherResponse ();

      response = await weather.GetWeatherForCoordinates (GPSLatitude.ToString (), GPSLongitude.ToString ());

      ImageModel.Temperature = response.Temperature.Temperature;

      await _context.SaveChangesAsync ();

      return Page ();

    }

        public async Task<IActionResult> OnPostAsync(int id, string comment, DateTime date, double gpslat,
            double gpslong, string material, int tracks, double temperature, string snow, string animalkind, string gender, string trackage,
            string collectedby) 
        {
            ImageModel = await _context.ImageData.FirstOrDefaultAsync(m => m.ID == id);

            ImageModel.Comment = comment;
            ImageModel.Date = date;
            ImageModel.Latitude = gpslat;
            ImageModel.Longitude = gpslong;
            ImageModel.Material = material;
            ImageModel.AmountOfTracksCollected = tracks;
            ImageModel.Temperature = temperature;
            ImageModel.Snow = snow;
            ImageModel.AnimalKind = animalkind;
            ImageModel.Gender = gender;
            ImageModel.TrackAge = trackage;
            ImageModel.CollectedBy = collectedby;

      if (ImageModel == null) {
        return NotFound ();
      }

      await _context.SaveChangesAsync ();

      return RedirectToPage ("./Index");

    }

  }
}