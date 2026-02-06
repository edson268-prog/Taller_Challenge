using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Infrastructure.Extensions;
using Taller_Challenge_Backend.Infrastructure.Helpers;
using Taller_Challenge_Backend.Infrastructure.Identity;
using Taller_Challenge_Backend.Infrastructure.Repositories;
using Taller_Challenge_Backend.Infrastructure.Services;

namespace Taller_Challenge_Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddPersistence(configuration, environment);

            // Identity and Authentication
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<JwtHelper>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            // Included HttpclientFactory for PricingService with retry policy
            services.AddHttpClient<IPricingService, PricingService>().ConfigureHttpClient((provider, client) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                client.BaseAddress = new Uri(config["PricingService:BaseUrl"]
                    ?? throw new InvalidOperationException("PricingService:BaseUrl not configured"));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(15);
            })
            .AddPolicyHandler(HttpPoliciesExtensions.GetRetryPolicy());

            return services;
        }
    }
}
