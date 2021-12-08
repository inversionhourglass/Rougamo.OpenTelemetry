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
        public bool SetOkStatusWhenSuccess { get; set; } = false;

        /// <summary>
        /// span using method full name by default, change to true to using short name and save full name to tag or event(depend on <see cref="ArgumentsStoreType"/>)
        /// </summary>
        public bool ShortName { get; set; } = false;

        /// <summary>
        /// <inheritdoc cref="OpenTelemetry.KeyNames"/>
        /// </summary>
        public KeyNames KeyNames { get; set; }
    }
}
