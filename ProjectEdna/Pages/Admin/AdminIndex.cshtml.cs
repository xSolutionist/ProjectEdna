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
    public class AdminIndexModel : PageModel {

        private readonly ProjectEdna.Data.ApplicationDbContext _context;

        public AdminIndexModel (ProjectEdna.Data.ApplicationDbContext context) {
            _context = context;
        }

        public IList<ProjectModel> ProjectModel { get; set; }

        public async Task OnGetAsync () {
            ProjectModel = await _context.Projects.ToListAsync ();
        }

    }
}