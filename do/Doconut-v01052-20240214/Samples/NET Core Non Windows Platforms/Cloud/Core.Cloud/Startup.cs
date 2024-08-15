using System.IO;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

/* Please ensure that the three DLLs from .NET Core Setup are referenced or copied to the bin folder */
using Doconut.Viewer.Middleware;
using Doconut.Config.Cloud;

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

namespace Doconut.Mvc.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpContextAccessor();

            // Required for caching
            services.AddMemoryCache();

            // For any encoding errors
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }


        // Webfarm 
        public static string webFarmFolder = "webfarm";

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
            app.UseCookiePolicy();

            app.MapWhen(
                context => context.Request.Path.ToString().EndsWith("DocImage.axd"),
                appBranch =>
                {
                    var cloudOptions = new Cloud.CloudOptions
                    {
                        PngPath = Path.Combine(env.WebRootPath, webFarmFolder),
                        Location = CloudLocation.CDN, // using CDN as the cloud provider
                        LogErrors = true,
                        LogPath = Path.Combine(env.WebRootPath, "logs"),
                        // optional, provide global CDN settings
                        CloudUploadConfig = new CDNConfig { DoconutCdnUrl = "http://cdn.doconut.com"  }
                    };

                    // specify this as true if you want to locally cache to disk
                    cloudOptions.FunctionsConfig.CachePages = false;
                    // also specify this as true if you want to locally cache to memory
                    cloudOptions.FunctionsConfig.CacheInMemory = false;

                    appBranch.UseDoconutCloud(cloudOptions);

                    // Uncomment to test without cloud
                    //appBranch.UseDoconut(new DoconutOptions { ShowDoconutInfo = true, UnSafeMode = false });

                });

            app.UseMvcWithDefaultRoute();
        }
    }
}
