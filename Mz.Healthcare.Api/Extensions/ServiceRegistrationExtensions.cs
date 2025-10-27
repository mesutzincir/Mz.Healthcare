using Microsoft.EntityFrameworkCore;
using Mz.Healthcare.Api.Data;
using Mz.Healthcare.Api.Services;
using Serilog;

namespace Mz.Healthcare.Api.Extensions;

public static class ServiceRegistrationExtensions
{
    /// <summary>
    ///     register the services
    /// </summary>
    /// <param name="services"> service collection for DI</param>
    /// <param name="configuration">Application configuration to get setting values</param>
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((_, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(configuration));

        services.AddScoped<IPatientService, PatientService>();
        services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("PatientDbConnection")
        );
    }
}