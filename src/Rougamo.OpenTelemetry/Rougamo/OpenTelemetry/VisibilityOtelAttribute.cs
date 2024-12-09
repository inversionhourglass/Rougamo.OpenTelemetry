using Rougamo.Metadatas;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// weave public method
    /// </summary>
    [Pointcut(AccessFlags.Public)]
    [Advice(Feature.Observe)]
    [Lifetime(Lifetime.Pooled)]
    public class PublicOtelAttribute : OtelAttribute
    {
    }

    /// <summary>
    /// weave public static method only
    /// </summary>
    [Pointcut(AccessFlags.StaticPublic)]
    [Advice(Feature.Observe)]
    [Lifetime(Lifetime.Pooled)]
    public class StaticOtelAttribute : OtelAttribute
    {
    }

    /// <summary>
    /// whatever visibility of method, weave all method
    /// </summary>
    [Pointcut(AccessFlags.All)]
    [Advice(Feature.Observe)]
    [Lifetime(Lifetime.Pooled)]
    public class FullOtelAttribute : OtelAttribute
    {
    }
}
