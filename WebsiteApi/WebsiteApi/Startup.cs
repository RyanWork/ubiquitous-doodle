using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebsiteApi.Common.ServiceAccountFactory;
using WebsiteApi.Model;

namespace WebsiteApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureRateLimiting(services);
            services.AddSingleton<IServiceAccountFactory, ServiceAccountFactory>();
            services.AddHttpClient();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebsiteApi", Version = "v1"
                });
            });
            
            services.Configure<AppSettings>(options => 
                _configuration.GetSection("AppSettings")
                    .Bind(options));

            var allowedOrigins = _configuration.GetSection("AllowedOrigins")
                .AsEnumerable()
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .Select(x => x.Value)
                .ToArray();
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

   	    services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });
        }

        private void ConfigureRateLimiting(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(_configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_configuration.GetSection("IpRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebsiteApi v1"));
            }

            var ipPolicyStore = app.ApplicationServices.GetRequiredService<IIpPolicyStore>();
            ipPolicyStore.SeedAsync().Wait();

            app.UseIpRateLimiting();

            app.UseHttpsRedirection();

            app.UseFileServer();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
