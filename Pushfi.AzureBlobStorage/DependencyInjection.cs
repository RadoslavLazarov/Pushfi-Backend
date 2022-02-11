using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pushfi.AzureBlobStorage.Interfaces;
using Pushfi.AzureBlobStorage.Services;
using Pushfi.Domain.Configuration;

namespace Pushfi.AzureBlobStorage
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureBlobStorageConfiguration>(configuration.GetSection("AzureBlobStorage"));
            services.AddTransient<IAzureBlobStorageService, AzureBlobStorageService>();

            return services;
        }
    }
}
