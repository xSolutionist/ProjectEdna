using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectEdna.Data;
using ProjectEdna.Model;
using QRCoder;

namespace ProjectEdna.Pages.Admin {
    [Authorize]
    public class CreateProjectModel : PageModel {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private IWebHostEnvironment webHostEnvironment;

        public CreateProjectModel (ApplicationDbContext context,
            IWebHostEnvironment iWebHostEnvironment,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) {
            _context = context;
            webHostEnvironment = iWebHostEnvironment;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [BindProperty]
        public ProjectModel ProjectModel { get; set; }

        public void OnGet () {

        }

        public async Task<IActionResult> OnPostFormAsync () {

            if (!ModelState.IsValid) {
                return Page ();

            }

            var user = await _userManager.GetUserAsync (User);

            ProjectModel.ProjectCreated = DateTime.Now;
            ProjectModel.ProjectCreator = User.Identity.Name;
            ProjectModel.UserId = user.Id;

            _context.Projects.Add (ProjectModel);
            await _context.SaveChangesAsync ();

            char A = ProjectModel.ProjectName.ToUpper ().First ();
            char B = ProjectModel.ProjectName.ToUpper ().Last ();
            char[] AB = { A, B };

            for (int i = 1; i <= ProjectModel.ProjectDataSize; i++) {
                string uniqueFileName = null;

                ImageModel imageModel = new ImageModel ();

                imageModel.Project = ProjectModel.ProjectName;
                imageModel.TestID = new string (AB) + i;
                imageModel.ProjectId = ProjectModel.Id;
                uniqueFileName = Guid.NewGuid ().ToString () + "_" + imageModel.TestID;

                imageModel.QRCode = uniqueFileName + ".jpg";

                var QRFolderPath = Path.Combine (webHostEnvironment.WebRootPath, "QRimages");
                if (!System.IO.Directory.Exists (QRFolderPath))
                    System.IO.Directory.CreateDirectory (QRFolderPath);

                var QRFilePath = Path.Combine (QRFolderPath, uniqueFileName);

                QRCodeGenerator qr = new QRCodeGenerator ();

                QRCodeData data = qr.CreateQrCode (uniqueFileName, QRCodeGenerator.ECCLevel.Q);

                QRCode code = new QRCode (data);

                Bitmap qrCodeImage = code.GetGraphic (20, Color.Black, Color.White, (Bitmap) Bitmap.FromFile (webHostEnvironment.WebRootPath + "/assets/dna.jpg"));
                qrCodeImage.Save (QRFilePath + ".jpg", ImageFormat.Jpeg);

                _context.ImageData.Add (imageModel);
                await _context.SaveChangesAsync ();

            }

            return RedirectToPage ("./AdminIndex");

        }
    }
}