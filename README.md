# Rougamo.OpenTelemetry

中文 | [English](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/blob/master/README_en.md)

`Rougamo.OpenTelemetry`是用来丰富[OpenTelemetry](https://github.com/open-telemetry/opentelemetry-dotnet)非IO埋点的组件，
其借助于[Rougamo](https://github.com/inversionhourglass/Rougamo)静态织入的功能，对指定方法增加`Trace`埋点

## 快速开始
在启动项目安装[Rougamo.OpenTelemetry.Hosting](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/tree/master/src/Rougamo.OpenTelemetry.Hosting),
然后在需要埋点的项目安装[Rougamo.OpenTelemetry](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/tree/master/src/Rougamo.OpenTelemetry)
```sh
dotnet add package Rougamo.OpenTelemetry.Hosting
dotnet add package Rougamo.OpenTelemetry
```
在启动项目`Startup.cs`中初始化`Rougamo.OpenTelemetry`
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .AddRougamoSource() // 初始化Rougamo.OpenTelemetry
                .AddAspNetCoreInstrumentation()
                .AddJaegerExporter();
        });

        // 修改Rougamo.OpenTelemetry默认配置
        services.AddOpenTelemetryRougamo(options =>
        {
            options.ArgumentsStoreType = ArgumentsStoreType.Tag;
        });
    }
}
```
之后便可以到项目中添加埋点，最简单的方式是通过在方法上添加`OtelAttribute`和`PureOtelAttribute`来为该方法添加埋点，两个Attribute的区别在于
`OtelAttribute`默认会记录参数和返回值，而`PureOtelAttribute`默认不会记录，当然，也可以分别配合`ApmIgnoreAttribute`和`ApmRecordAttribute`
忽略或记录指定参数或返回值
```csharp
class Service
{
    [return: ApmIgnore]     // 返回值不记录
    [Otel] // 默认记录参数和返回值，需要通过ApmIgnoreAttribute来忽略不需要记录的参数或返回值
    public async Task<string> M1(
            [ApmIgnore] string uid, // 该参数不记录
            DateTime time)
    {
        // do something
        return string.Empty;
    }

    [PureOtel] // 默认不记录参数和返回值，需要通过ApmRecordAttribute来记录指定的参数或返回值
    public void M2(
            [ApmRecord] double d1,  // 记录该参数
            double d2)
    {
        // do something
    }
}
```
默认情况下，参数和返回值会记录到当前Span的Event中，可以像上面`Startup`示例代码那样修改参数和返回值存放位置，
当上面M1或M2方法被调用时便会生成一个对应的Span记录

## 通过接口批量织入
`OpenTelemetry`的本地埋点是通过[Rougamo](https://github.com/inversionhourglass/Rougamo)进行静态织入的，这也就表示我们可以使用`Rougamo`提供的各种
代码织入方法，所以我们可以使用[实现空接口的方式进行织入](https://github.com/inversionhourglass/Rougamo#%E9%80%9A%E8%BF%87%E5%AE%9E%E7%8E%B0%E7%A9%BA%E6%8E%A5%E5%8F%A3%E7%9A%84%E6%96%B9%E5%BC%8F%E8%BF%9B%E8%A1%8C%E7%BB%87%E5%85%A5irougamo)，
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
上面的代码中，由于`ITestService`实现了`IRougamo<>`泛型接口，并且制定`OtelAttribute`为泛型类型，所以`ITestService`的实现类`TestService`的
所有公开(public)实例(非static)方法将自动织入`OtelAttribute`的实现，这里之所以是公开实例方法，是因为`OtelAttribute.Flags`属性值默认为`AccessFlags.InstancePublic`，
除了公开实例的可访问性注入，下面列出了其他可访问性的Attribute，如果下面的可访问性还不能满足要求，可以根据自己的需求自己继承`OtelAttribute`或`PureOtelAttribute`，
然后重写`Flags`属性

|最小版本要求|默认记录参数和返回值|默认不记录参数和返回值|可访问性|
|:--:|:--:|:--:|:--|
|0.1.2|PublicOtelAttribute|PublicPureOtelAttribute|所有public方法，无论是静态方法还是实例方法|
|0.1.2|StaticOtelAttribute|StaticPureOtelAttribute|所有public static方法|
|0.1.2|FullOtelAttribute|FullPureOtelAttribute|所有方法，不论可访问性如何|

## 通过代理Attribute织入
[代理Attribute织入](https://github.com/inversionhourglass/Rougamo#attribute%E4%BB%A3%E7%90%86%E7%BB%87%E5%85%A5moproxyattribute)同样是`Rougamo`提供的织入方式，
其中一种使用场景是结合[Rougamo.APM.Abstractions](https://github.com/inversionhourglass/Rougamo.APM#rougamoapmabstractions)使用，埋点的Attribute使用`Rougamo.APM.Abstractions`
提供的`SpanAttribute`和`PureSpanAttribute`，这两个Attribute并不会织入代码，仅仅用于标记，之后在通过代理的方式进行真正的织入
```csharp
// Startup.cs或者AssemblyInfo.cs等项目其他任意位置添加下面的程序集级别的Attribute
[assembly: MoProxy(typeof(SpanAttribute), typeof(OtelAttribute))]
[assembly: MoProxy(typeof(PureSpanAttribute), typeof(PureOtelAttribute))]

public class Cls
{
    [Span]  // 最终通过代理织入OtelAttribute的代码实现
    public int M1()
    {
        // ...
        return 123;
    }

    [PureSpan]  // 最终通过代理织入PureOtelAttribute的代码实现
    private async Task<string> M2Async()
    {
        // ...
        return string.Empty;
    }
}
```
通过代理的方式无法使用接口织入，也无法自定义参数（OtelAttribute.Name），但使用这种方式的优势在于，如果你还不确定你的APM最终使用什么或者可能会换其他`OpenTelemetry`不支持的APM，
那么你使用这种代理的方式就可以仅仅修改上面实例代码中`MoProxyAttribute`指定代理类型的代码。
