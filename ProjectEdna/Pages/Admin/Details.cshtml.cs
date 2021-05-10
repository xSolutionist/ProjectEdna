using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectEdna.Data;
using ProjectEdna.Model;

namespace ProjectEdna.Pages.Admin {
    public class DetailsModel : PageModel {
        private readonly ProjectEdna.Data.ApplicationDbContext _context;

        public DetailsModel (ProjectEdna.Data.ApplicationDbContext context) {
            _context = context;
        }

        public ImageModel ImageModel { get; set; }

        public async Task<IActionResult> OnGetAsync (int? id) {
            if (id == null) {
                return NotFound ();
            }

            ImageModel = await _context.ImageData.FirstOrDefaultAsync (m => m.ID == id);

            if (ImageModel == null) {
                return NotFound ();
            }
            return Page ();
        }
    }
}