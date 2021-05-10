using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectEdna.Data;
using ProjectEdna.Model;

namespace ProjectEdna.Pages.CollectData
{
    public class CollectDataIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private IWebHostEnvironment webHostEnvironment;

        public CollectDataIndexModel(ApplicationDbContext context, IWebHostEnvironment iWebHostEnvironment,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)

        {
            _context = context;
            webHostEnvironment = iWebHostEnvironment;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [BindProperty]
        public ImageModel ImageModel { get; set; }


        public string QR { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostSaveAsync(string imageBase64, double? Lat, double? Long)
        {
            
            QR = ImageModel.QRCode + ".jpg";
            ImageModel = await _context.ImageData.FirstOrDefaultAsync(m => m.QRCode == QR);

            string uniqueFileName = null;

            var imagesFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
            if (!System.IO.Directory.Exists(imagesFolderPath))
                System.IO.Directory.CreateDirectory(imagesFolderPath);
            uniqueFileName = Guid.NewGuid().ToString() + ".jpg"; // Genererar unika filnamn 
            var imagesFilePath = Path.Combine(imagesFolderPath, uniqueFileName);


            string[] imageInputString = imageBase64.Split(',');



            var bytes = Convert.FromBase64String(imageInputString[1]);

            using (var imageFile = new FileStream(imagesFilePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }


          

                ImageModel.Image = uniqueFileName;
                ImageModel.Latitude = Lat;
                ImageModel.Longitude = Long;

           




            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(ImageModel).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageModelExists(ImageModel.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ValidateCollectedData", new { id = ImageModel.ID });
        }

        private bool ImageModelExists(int id)
        {
            return _context.ImageData.Any(e => e.ID == id);
        }
    }
}
