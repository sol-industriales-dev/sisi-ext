
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Doconut.Viewer.Middleware;

/*
   Important: For DOCX, ODT, XML, or RTF files, using .NET Core Standard and NET 6,
              add SkiaSharp 2.88.6.
              NuGet Package: https://www.nuget.org/packages/SkiaSharp/2.88.6 

   Important: For PSD files using .NET Core Standard and NET 6,
              add System.Drawing.Common 6.0.0. and System.Text.Encoding.CodePages 6.0.0.
              NuGet Packages:
                 - https://www.nuget.org/packages/System.Drawing.Common/6.0.0 
                 - https://www.nuget.org/packages/System.Text.Encoding.CodePages/6.0.0 

   Important: For CAD file types using .NET Core Standard,
              add System.Text.Json 6.0.0., System.Drawing.Common 6.0.0., and System.Text.Encoding.CodePages 6.0.0.
              NuGet Packages:
                 - https://www.nuget.org/packages/System.Text.Json/6.0.0 
                 - https://www.nuget.org/packages/System.Drawing.Common/6.0.0 
                 - https://www.nuget.org/packages/System.Text.Encoding.CodePages/6.0.0 
*/

namespace Doconut.Angular.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            // Required for Doconut
            app.MapWhen(
                context => context.Request.Path.ToString().EndsWith("DocImage.axd"),
                appBranch =>
                {
                    appBranch.UseDoconut(new DoconutOptions { UnSafeMode = false, ShowDoconutInfo = true });
                });

            // Angular
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.StartupTimeout = new System.TimeSpan(0, 1, 0);

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
