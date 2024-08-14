# Rougamo.OpenTelemetry

`Rougamo.OpenTelemetry` is a component designed to enrich [OpenTelemetry](https://github.com/open-telemetry/opentelemetry-dotnet) with non-IO instrumentation. It leverages the static weaving capabilities of [Rougamo](https://github.com/inversionhourglass/Rougamo) to add `Trace` instrumentation to specified methods.

## Quick Start

First, install [Rougamo.OpenTelemetry.Hosting](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/tree/master/src/Rougamo.OpenTelemetry.Hosting) in the startup project, and then install [Rougamo.OpenTelemetry](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/tree/master/src/Rougamo.OpenTelemetry) in the project where you want to add instrumentation:

```sh
dotnet add package Rougamo.OpenTelemetry.Hosting
dotnet add package Rougamo.OpenTelemetry
```

In the `Startup.cs` of the startup project, initialize `Rougamo.OpenTelemetry`:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .AddRougamoSource() // Initialize Rougamo.OpenTelemetry
                .AddAspNetCoreInstrumentation()
                .AddJaegerExporter();
        });

        // Modify Rougamo.OpenTelemetry default configuration
        services.AddOpenTelemetryRougamo(options =>
        {
            options.ArgumentsStoreType = ArgumentsStoreType.Tag;
        });
    }
}
```

After setup, you can add instrumentation to your project. The simplest way is to use `OtelAttribute` and `PureOtelAttribute` on methods to add instrumentation. The difference between the two attributes is that `OtelAttribute` records parameters and return values by default, while `PureOtelAttribute` does not. You can also use `ApmIgnoreAttribute` and `ApmRecordAttribute` to ignore or record specific parameters or return values.

```csharp
class Service
{
    [return: ApmIgnore]     // Do not record the return value
    [Otel] // Records parameters and return values by default, use ApmIgnoreAttribute to ignore parameters or return values
    public async Task<string> M1(
            [ApmIgnore] string uid, // Do not record this parameter
            DateTime time)
    {
        // do something
        return string.Empty;
    }

    [PureOtel] // Does not record parameters and return values by default, use ApmRecordAttribute to record specified parameters or return values
    public void M2(
            [ApmRecord] double d1,  // Record this parameter
            double d2)
    {
        // do something
    }
}
```

By default, parameters and return values are recorded as events in the current Span. You can modify where these are stored, as shown in the `Startup` example above. When methods like `M1` or `M2` are called, a corresponding Span will be created to record the data.

## Bulk Weaving via Interfaces

Local instrumentation with `OpenTelemetry` is performed using static weaving via [Rougamo](https://github.com/inversionhourglass/Rougamo). This means you can use various code weaving methods provided by `Rougamo`. For example, you can use the [empty interface approach for weaving](https://github.com/inversionhourglass/Rougamo#%E9%80%9A%E8%BF%87%E5%AE%9E%E7%8E%B0%E7%A9%BA%E6%8E%A5%E5%8F%A3%E7%9A%84%E6%96%B9%E5%BC%8F%E8%BF%9B%E8%A1%8C%E7%BB%87%E5%85%A5irougamo):

```csharp
public interface ITestService : IRougamo<OtelAttribute>
{
    // ...
}
public class TestService : ITestService
{
    // ...
}
```

In the code above, since `ITestService` implements the `IRougamo<>` generic interface and specifies `OtelAttribute` as the generic type, all public (non-static) instance methods of `TestService`, the implementation class, will automatically have `OtelAttribute` woven into them. The default `OtelAttribute.Flags` property value is `AccessFlags.InstancePublic`, which means it injects into all public instance methods. Other accessibility attributes are listed below. If these do not meet your needs, you can inherit `OtelAttribute` or `PureOtelAttribute` and override the `Flags` property as needed.

| Minimum Version | Default Records Parameters and Return Values | Default Does Not Record Parameters and Return Values | Accessibility |
|:--:|:--:|:--:|:--:|
| 0.1.2 | PublicOtelAttribute | PublicPureOtelAttribute | All public methods, whether static or instance |
| 0.1.2 | StaticOtelAttribute | StaticPureOtelAttribute | All public static methods |
| 0.1.2 | FullOtelAttribute | FullPureOtelAttribute | All methods, regardless of accessibility |

## Weaving via Proxy Attributes

[Proxy attribute weaving](https://github.com/inversionhourglass/Rougamo#attribute%E4%BB%A3%E7%90%86%E7%BB%87%E5%85%A5moproxyattribute) is another weaving method provided by `Rougamo`. One use case is to combine with [Rougamo.APM.Abstractions](https://github.com/inversionhourglass/Rougamo.APM#rougamoapmabstractions). The attributes used for instrumentation, such as `SpanAttribute` and `PureSpanAttribute`, do not weave code themselves but are markers. Actual weaving is done via proxies.

```csharp
// Add the following assembly-level attributes in Startup.cs or AssemblyInfo.cs
[assembly: MoProxy(typeof(SpanAttribute), typeof(OtelAttribute))]
[assembly: MoProxy(typeof(PureSpanAttribute), typeof(PureOtelAttribute))]

public class Cls
{
    [Span]  // Finally, the code implementation of OtelAttribute is woven via proxy
    public int M1()
    {
        // ...
        return 123;
    }

    [PureSpan]  // Finally, the code implementation of PureOtelAttribute is woven via proxy
    private async Task<string> M2Async()
    {
        // ...
        return string.Empty;
    }
}
```

Proxy-based weaving does not support interface weaving and cannot customize parameters (e.g., `OtelAttribute.Name`). However, the advantage of using this approach is that if you are unsure of your final APM or may switch to another APM not supported by `OpenTelemetry`, you only need to modify the `MoProxyAttribute` specification in the example code.
