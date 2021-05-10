using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectEdna.Data;
using ProjectEdna.Model;
namespace ProjectEdna.Pages {
    public class CreateModel : PageModel {
        private readonly ProjectEdna.Data.ApplicationDbContext _context;

        public CreateModel (ProjectEdna.Data.ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult OnGet () {
            return Page ();
        }

        [BindProperty]
        public ImageModel ImageModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync () {
            if (!ModelState.IsValid) {
                return Page ();
            }

            _context.ImageData.Add (ImageModel);
            await _context.SaveChangesAsync ();

            return RedirectToPage ("./Index");
        }
    }
}