using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebsiteApi
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        static async Task Main(string[] args)
        {
            using IWebHost webHost = CreateHostBuilder().Build();
            await webHost.StartAsync();
            await webHost.WaitForShutdownAsync();
        }

        private static IWebHostBuilder CreateHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseKestrel(options => 
		{
			options.Listen(IPAddress.Any, 80);
			options.Listen(IPAddress.Loopback, 443, listenOptions =>
			{
				listenOptions.UseHttps("/certs/certificate.pfx", "");
			});
		})
                .UseStartup<Startup>();
    }
}
