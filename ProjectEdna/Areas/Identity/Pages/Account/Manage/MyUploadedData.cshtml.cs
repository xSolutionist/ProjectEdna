using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectEdna.Model;

namespace ProjectEdna.Areas.Identity.Pages.Account.Manage
{
    public class MyUploadedData : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ProjectEdna.Data.ApplicationDbContext _context;

        public MyUploadedData(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<ChangePasswordModel> logger,
            ProjectEdna.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }


        [TempData]
        public string StatusMessage { get; set; }







        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ImageModel = await _context.ImageData.Where(p => p.UserId == user.Id)
                .ToListAsync();

            return Page();
        }
        public IList<ImageModel> ImageModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            
            return RedirectToPage();
        }
    }
}
