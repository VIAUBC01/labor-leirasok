using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieCatalog.Data;
using System.Threading.Tasks;
using static System.Globalization.CultureInfo;

namespace MovieCatalog.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DefaultThreadCurrentCulture = DefaultThreadCurrentUICulture = InvariantCulture;
            await (await CreateHostBuilder(args).Build().MigrateAndSeedDataAsync()).RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureServices((context, services) => services
                        .AddMovieDataService()
                        .AddDbContext<MovieCatalogDbContext>(options => options
                            .UseSqlServer(context.Configuration.GetConnectionString("MovieCatalog")))
                        .AddRazorPages())
                    .Configure((context, app) =>
                        (!context.HostingEnvironment.IsDevelopment()
                            ? app.UseExceptionHandler("/Error")
                                .UseHsts()
                            : app.UseDeveloperExceptionPage())
                            .UseHttpsRedirection()
                            .UseStaticFiles()
                            .UseRouting()
                            .UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                                endpoints.MapRazorPages();
                            })));
    }
}