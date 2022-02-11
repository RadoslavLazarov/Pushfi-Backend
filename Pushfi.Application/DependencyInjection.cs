using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pushfi.Domain.Configuration;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Pushfi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
            services.Configure<EnfortraConfiguration>(configuration.GetSection("Enfortra"));
            services.Configure<SendGridConfiguration>(configuration.GetSection("SendGrid"));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
