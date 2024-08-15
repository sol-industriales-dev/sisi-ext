using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace Doconut.Mvc.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
