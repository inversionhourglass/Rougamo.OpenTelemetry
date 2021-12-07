using OpenTelemetry.Trace;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// options
    /// </summary>
    public class OtelRougamoOptions
    {
        /// <summary>
        /// how to store arguments
        /// </summary>
        public ArgumentsStoreType ArgumentsStoreType { get; set; } = ArgumentsStoreType.Event;

        /// <summary>
        /// default false that use <see cref="Status.Unset"/> when span/activity execute without any exception throws,
        /// change to true to use <see cref="Status.Ok"/>
        /// </summary>
        public bool SetOkStatusWhenSuccess = false;
    }
}
