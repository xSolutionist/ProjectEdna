using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectEdna.Data;
using ProjectEdnaWeb.Services;

namespace ProjectEdna {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext> (options =>
                options.UseSqlServer (
                    Configuration.GetConnectionString ("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter ();
            services.AddDefaultIdentity<IdentityUser> (options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext> ();

            services.AddTransient<IEmailSender, EmailSender> ();
            services.Configure<AuthMessageSenderOptions> (Configuration);

            services.AddRazorPages ();
            services.Configure<CookiePolicyOptions> (options => {

                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            services.AddAuthentication ()
                .AddFacebook (options => {

                    options.AppId = "543569466621175"; // Configuration["App:FacebookClientId"]
                    options.ClientSecret = "095bf7d27d09850ec9364ced7c7e2d08"; // Configuration["App:FacebookClientSecret"]

                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseMigrationsEndPoint ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();
            app.UseCookiePolicy ();

            app.UseEndpoints (endpoints => {
                endpoints.MapRazorPages ();
            });
        }
    }
}