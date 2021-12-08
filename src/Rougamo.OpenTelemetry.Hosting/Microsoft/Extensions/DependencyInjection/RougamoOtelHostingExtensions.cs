using Rougamo.OpenTelemetry;
using Rougamo.OpenTelemetry.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class RougamoOtelHostingExtensions
    {
        /// <summary>
        /// add open telemetry rougamo annotation weave support
        /// </summary>
        public static IServiceCollection AddOpenTelemetryRougamo(this IServiceCollection services, Action<OtelRougamoOptions> configureOptions)
        {
            services.AddOptions<OtelRougamoOptions>().Configure(configureOptions);

            return services.AddOpenTelemetryRougamo();
        }

        /// <summary>
        /// add open telemetry rougamo annotation weave support
        /// </summary>
        public static IServiceCollection AddOpenTelemetryRougamo(this IServiceCollection services)
        {
            return services.AddHostedService<SingletonInitialHostedService>();
        }
    }
}
