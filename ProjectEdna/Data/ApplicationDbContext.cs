using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectEdna.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEdna.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       public DbSet<ImageModel> ImageData { get; set; }

        public DbSet<ProjectModel> Projects { get; set; }

    }
}
