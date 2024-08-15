using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/* Please ensure that the three DLLs from .NET Core Setup are referenced or copied to the bin folder */
using Doconut.Viewer.Middleware;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;

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

namespace DoconutWebApp
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
            services.AddRazorPages();

            services.AddHttpContextAccessor();

            // For any encoding errors
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddMemoryCache();

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
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
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapWhen(
               context => context.Request.Path.ToString().EndsWith("DocImage.axd"),
               appBranch => {

                   appBranch.UseDoconut(new DoconutOptions { UnSafeMode = false, ShowDoconutInfo = true });

                   // For {webfarm} 'comment' above line and 'uncomment' below line
                   // (also refer docViewer.OpenDocument in HomeController.cs) 

                   // create a cache folder in wwwroot
                   // appBranch.UseDoconutWebFarm(new WebFarmOptions { Path = Path.Combine(env.WebRootPath, "cache") }); 

               });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
