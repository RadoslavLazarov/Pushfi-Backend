using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Infrastructure.Persistence;
using Pushfi.Infrastructure.Services;

namespace Pushfi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            //services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEnfortraService, EnfortraService>();
            services.AddTransient<ISoapService, SoapService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IJwtService, JwtService>();

            return services;
        }
    }
}
