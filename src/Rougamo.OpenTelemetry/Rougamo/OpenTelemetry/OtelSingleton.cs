using Rougamo.APM.Serialization;
using System.Diagnostics;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// singleton instances
    /// </summary>
    public class OtelSingleton
    {
        /// <summary>
        /// <see cref="ActivitySource"/>
        /// </summary>
        internal static readonly ActivitySource Source = new ActivitySource(OtelConstants.ACTIVITY_SOURCE_NAME);

        /// <summary>
        /// parameter and return value serializer, default <see cref="ToStringSerializer"/>
        /// </summary>
        public static ISerializer Serializer = new ToStringSerializer();

        /// <summary>
        /// options
        /// </summary>
        public static OtelRougamoOptions Options = new OtelRougamoOptions();
    }
}
