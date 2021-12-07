using Rougamo.OpenTelemetry;

namespace OpenTelemetry.Trace
{
    /// <summary>
    /// <see cref="TracerProviderBuilder"/> extensions
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// add rougamo source
        /// </summary>
        public static TracerProviderBuilder AddRougamoSource(this TracerProviderBuilder builder)
        {
            return builder.AddSource(OtelConstants.ACTIVITY_SOURCE_NAME);
        }
    }
}
