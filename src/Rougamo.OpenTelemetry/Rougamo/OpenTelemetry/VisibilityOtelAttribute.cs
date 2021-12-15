namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// weave public method
    /// </summary>
    public class PublicOtelAttribute : OtelAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override AccessFlags Flags => AccessFlags.Public;
    }

    /// <summary>
    /// weave public static method only
    /// </summary>
    public class StaticOtelAttribute : OtelAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override AccessFlags Flags => AccessFlags.StaticPublic;
    }

    /// <summary>
    /// whatever visibility of method, weave all method
    /// </summary>
    public class FullOtelAttribute : OtelAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override AccessFlags Flags => AccessFlags.All;
    }
}
