using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExifLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectEdna.Data;
using ProjectEdna.ExtensionMethods;
using ProjectEdna.Model;

namespace ProjectEdna.Pages {
    public class UploadModel : PageModel {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private IWebHostEnvironment webHostEnvironment;

        public UploadModel (ApplicationDbContext context, IWebHostEnvironment iWebHostEnvironment,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)

        {
            _context = context;
            webHostEnvironment = iWebHostEnvironment;
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

            QRFileName = ImageModel.QRCode;

            if (ImageModel == null) {
                return NotFound ();
            }
            return Page ();

        }

        public async Task<IActionResult> OnPostPhotoAsync (IFormFile photo, string QR) {

            ImageModel = await _context.ImageData.FirstOrDefaultAsync (m => m.QRCode == QR);

            if (ImageModel == null) {
                return NotFound ();
            }

            string uniqueFileName = null;

            var imagesFolderPath = Path.Combine (webHostEnvironment.WebRootPath, "images");
            if (!System.IO.Directory.Exists (imagesFolderPath))
                System.IO.Directory.CreateDirectory (imagesFolderPath);
            uniqueFileName = Guid.NewGuid ().ToString () + "_" + photo.FileName; // Genererar unika filnamn 
            var imagesFilePath = Path.Combine (imagesFolderPath, uniqueFileName);

            using (var stream = new FileStream (imagesFilePath, FileMode.Create)) {
                photo.CopyTo (stream); // tog bort Async Den fucka ur f�r mig

            }

            try {

                using (var reader = new ExifReader (imagesFilePath)) {

                    reader.GetTagValue (ExifTags.DateTimeDigitized, out DateTime photoDate);
                    reader.GetTagValue (ExifTags.GPSAltitude, out double GpsAltitude);
                    reader.GetTagValue (ExifTags.GPSLatitude, out double[] GpsLatitude);

                    GPSLatitude = reader.GetLatitude ();
                    GPSLongitude = reader.GetLongitude ();
                    GPSDate = photoDate;

                    ImageModel.Latitude = reader.GetLatitude ();
                    ImageModel.Longitude = reader.GetLongitude ();
                    ImageModel.Date = photoDate;
                    ImageModel.Altitude = GpsAltitude;

                    DataExtracted = true;
                }
            } catch {

            }

            ImageModel.Image = uniqueFileName;

            ImageModel.CollectedBy = User.Identity.Name;

            if (!ModelState.IsValid) {
                return Page ();
            }

            //  _context.Attach(ImageModel).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!ImageModelExists (ImageModel.ID)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return RedirectToPage ("./SampleData", new { id = ImageModel.ID });
        }

        private bool ImageModelExists (int id) {
            return _context.ImageData.Any (e => e.ID == id);
        }
    }

}