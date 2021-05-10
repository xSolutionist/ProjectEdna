using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectEdna.Data;
using ProjectEdna.Model;

namespace ProjectEdna.Pages.Admin {
    public class EditModel : PageModel {
        private readonly ProjectEdna.Data.ApplicationDbContext _context;

        public EditModel (ProjectEdna.Data.ApplicationDbContext context) {
            _context = context;
        }

        [BindProperty]
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync () {
            if (!ModelState.IsValid) {
                return Page ();
            }

            _context.Attach (ImageModel).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!ImageModelExists (ImageModel.ID)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return RedirectToPage ("./AdminIndex");
        }

        private bool ImageModelExists (int id) {
            return _context.ImageData.Any (e => e.ID == id);
        }
    }
}