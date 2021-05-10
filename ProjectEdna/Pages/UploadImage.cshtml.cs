using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using ExifLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectEdna.Data;
using ProjectEdna.ExtensionMethods;
using ProjectEdna.Model;
using QRCoder;

namespace ProjectEdna.Pages {
    



    public class UploadImageModel : PageModel {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private IWebHostEnvironment webHostEnvironment;

        public UploadImageModel (ApplicationDbContext context, IWebHostEnvironment iWebHostEnvironment,
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

        public void OnGet () {

        }

        public void OnPostPhotoAsync (IFormFile photo) {
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
            var QRFolderPath = Path.Combine (webHostEnvironment.WebRootPath, "QRimages");
            if (!System.IO.Directory.Exists (QRFolderPath))
                System.IO.Directory.CreateDirectory (QRFolderPath);

            var QRFilePath = Path.Combine (QRFolderPath, uniqueFileName);

            QRCodeGenerator qr = new QRCodeGenerator ();

            QRCodeData data = qr.CreateQrCode (uniqueFileName, QRCodeGenerator.ECCLevel.Q);

            QRCode code = new QRCode (data);

            Bitmap qrCodeImage = code.GetGraphic (20, Color.Black, Color.White, (Bitmap) Bitmap.FromFile (webHostEnvironment.WebRootPath + "/assets/dna.jpg"));
            qrCodeImage.Save (QRFilePath, ImageFormat.Jpeg);

            ImageFileName = uniqueFileName;

            ImageModel.Image = uniqueFileName;
            ImageModel.QRCode = uniqueFileName;
            ImageModel.CollectedBy = User.Identity.Name;

        }

        public async Task<IActionResult> OnPostFormAsync () {

            if (!ModelState.IsValid) {
                return Page ();

            }

            var user = await _userManager.GetUserAsync (User);

            ImageModel.UserId = user.Id;

            _context.ImageData.Add (ImageModel);
            await _context.SaveChangesAsync ();

            return RedirectToPage ("./Index");

        }

    }
}