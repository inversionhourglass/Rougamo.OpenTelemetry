﻿using Rougamo.Metadatas;

namespace Rougamo.OpenTelemetry
{
    /// <summary>
    /// pure weave public method
    /// </summary>
    [Pointcut(AccessFlags.Public)]
    [Lifetime(Lifetime.Pooled)]
    public class PublicPureOtelAttribute : PureOtelAttribute
    {
    }

    /// <summary>
    /// pure weave public static method only
    /// </summary>
    [Pointcut(AccessFlags.StaticPublic)]
    [Lifetime(Lifetime.Pooled)]
    public class StaticPureOtelAttribute : PureOtelAttribute
    {
    }

    /// <summary>
    /// whatever visibility of method, pure weave all method
    /// </summary>
    [Pointcut(AccessFlags.All)]
    [Lifetime(Lifetime.Pooled)]
    public class FullPureOtelAttribute : PureOtelAttribute
    {
    }
}
