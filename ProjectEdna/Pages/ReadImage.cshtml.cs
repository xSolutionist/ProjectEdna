using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectEdna.Data;
using ProjectEdna.Model;

namespace ProjectEdna.Pages {
    public class ReadImageModel : PageModel {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private IWebHostEnvironment webHostEnvironment;

        public ReadImageModel (ApplicationDbContext context, IWebHostEnvironment iWebHostEnvironment,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)

        {
            _context = context;
            webHostEnvironment = iWebHostEnvironment;
            _signInManager = signInManager;
            _userManager = userManager;

        }

      
        public ImageModel ImageModel { get; set; }

        public string QR { get; set; }

        public void OnGet () {

        }

        public async Task<IActionResult> OnPostQRAsync () {
            QR = ImageModel.QRCode + ".jpg";

            ImageModel = await _context.ImageData.FirstOrDefaultAsync (m => m.QRCode == QR);

            if (!ModelState.IsValid) {
                return Page ();
            }

            //_context.Attach(ImageModel).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!ImageModelExists (ImageModel.ID)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return RedirectToPage ("./Upload", new { id = ImageModel.ID });

        }

        private bool ImageModelExists (int id) {
            return _context.ImageData.Any (e => e.ID == id);
        }
    }

}