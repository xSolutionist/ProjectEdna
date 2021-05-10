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
    public class IndexModel : PageModel {
        private readonly ProjectEdna.Data.ApplicationDbContext _context;

        public IndexModel (ProjectEdna.Data.ApplicationDbContext context) {
            _context = context;
        }

        public IList<ImageModel> ImageModel { get; set; }

        public async Task OnGetAsync (int? id) {
            ImageModel = await _context.ImageData.Where (m => m.ProjectId == id)
                .ToListAsync ();
        }
    }
}