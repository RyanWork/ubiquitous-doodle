using System.Diagnostics.CodeAnalysis;
using System.IO;
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
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
