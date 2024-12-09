using Rougamo.Metadatas;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// weave public method
    /// </summary>
    [Pointcut(AccessFlags.Public)]
    [Lifetime(Lifetime.Pooled)]
    public class PublicOtelAttribute : OtelAttribute
    {
    }

    /// <summary>
    /// weave public static method only
    /// </summary>
    [Pointcut(AccessFlags.StaticPublic)]
    [Lifetime(Lifetime.Pooled)]
    public class StaticOtelAttribute : OtelAttribute
    {
    }

    /// <summary>
    /// whatever visibility of method, weave all method
    /// </summary>
    [Pointcut(AccessFlags.All)]
    [Lifetime(Lifetime.Pooled)]
    public class FullOtelAttribute : OtelAttribute
    {
    }
}
