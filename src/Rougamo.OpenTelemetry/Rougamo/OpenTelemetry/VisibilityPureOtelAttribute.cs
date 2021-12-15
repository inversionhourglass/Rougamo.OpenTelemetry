namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// pure weave public method
    /// </summary>
    public class PublicPureOtelAttribute : PureOtelAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override AccessFlags Flags => AccessFlags.Public;
    }

    /// <summary>
    /// pure weave public static method only
    /// </summary>
    public class StaticPureOtelAttribute : PureOtelAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override AccessFlags Flags => AccessFlags.StaticPublic;
    }

    /// <summary>
    /// whatever visibility of method, pure weave all method
    /// </summary>
    public class FullPureOtelAttribute : PureOtelAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override AccessFlags Flags => AccessFlags.All;
    }
}
