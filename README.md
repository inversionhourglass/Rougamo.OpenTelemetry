# Rougamo.OpenTelemetry
`Rougamo.OpenTelemetry`�������ḻ[OpenTelemetry](https://github.com/open-telemetry/opentelemetry-dotnet)��IO���������
�������[Rougamo](https://github.com/inversionhourglass/Rougamo)��̬֯��Ĺ��ܣ���ָ����������`Trace`���

## ���ٿ�ʼ
��������Ŀ��װ[Rougamo.OpenTelemetry.Hosting](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/tree/master/src/Rougamo.OpenTelemetry.Hosting),
Ȼ������Ҫ������Ŀ��װ[Rougamo.OpenTelemetry](https://github.com/inversionhourglass/Rougamo.OpenTelemetry/tree/master/src/Rougamo.OpenTelemetry)
```sh
dotnet add package Rougamo.OpenTelemetry.Hosting
dotnet add package Rougamo.OpenTelemetry
```
��������Ŀ`Startup.cs`�г�ʼ��`Rougamo.OpenTelemetry`
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .AddRougamoSource() // ��ʼ��Rougamo.OpenTelemetry
                .AddAspNetCoreInstrumentation()
                .AddJaegerExporter();
        });

        // �޸�Rougamo.OpenTelemetryĬ������
        services.AddOpenTelemetryRougamo(options =>
        {
            options.ArgumentsStoreType = ArgumentsStoreType.Tag;
        });
    }
}
```
֮�����Ե���Ŀ�������㣬��򵥵ķ�ʽ��ͨ���ڷ��������`OtelAttribute`��`PureOtelAttribute`��Ϊ�÷��������㣬����Attribute����������
`OtelAttribute`Ĭ�ϻ��¼�����ͷ���ֵ����`PureOtelAttribute`Ĭ�ϲ����¼����Ȼ��Ҳ���Էֱ����`ApmIgnoreAttribute`��`ApmRecordAttribute`
���Ի��¼ָ�������򷵻�ֵ
```csharp
class Service
{
    [return: ApmIgnore]     // ����ֵ����¼
    [Otel] // Ĭ�ϼ�¼�����ͷ���ֵ����Ҫͨ��ApmIgnoreAttribute�����Բ���Ҫ��¼�Ĳ����򷵻�ֵ
    public async Task<string> M1(
            [ApmIgnore] string uid, // �ò�������¼
            DateTime time)
    {
        // do something
        return string.Empty;
    }

    [PureOtel] // Ĭ�ϲ���¼�����ͷ���ֵ����Ҫͨ��ApmRecordAttribute����¼ָ���Ĳ����򷵻�ֵ
    public void M2(
            [ApmRecord] double d1,  // ��¼�ò���
            double d2)
    {
        // do something
    }
}
```
Ĭ������£������ͷ���ֵ���¼����ǰSpan��Event�У�����������`Startup`ʾ�����������޸Ĳ����ͷ���ֵ���λ�ã�
������M1��M2����������ʱ�������һ����Ӧ��Span��¼

## ͨ���ӿ�����֯��
`OpenTelemetry`�ı��������ͨ��[Rougamo](https://github.com/inversionhourglass/Rougamo)���о�̬֯��ģ���Ҳ�ͱ�ʾ���ǿ���ʹ��`Rougamo`�ṩ�ĸ���
����֯�뷽�����������ǿ���ʹ��[ʵ�ֿսӿڵķ�ʽ����֯��](https://github.com/inversionhourglass/Rougamo#%E9%80%9A%E8%BF%87%E5%AE%9E%E7%8E%B0%E7%A9%BA%E6%8E%A5%E5%8F%A3%E7%9A%84%E6%96%B9%E5%BC%8F%E8%BF%9B%E8%A1%8C%E7%BB%87%E5%85%A5irougamo)��
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
����Ĵ����У�����`ITestService`ʵ����`IRougamo<>`���ͽӿڣ������ƶ�`OtelAttribute`Ϊ�������ͣ�����`ITestService`��ʵ����`TestService`��
���й���(public)ʵ��(��static)�������Զ�֯��`OtelAttribute`��ʵ�֣�����֮�����ǹ���ʵ������������Ϊ`OtelAttribute.Flags`����ֵĬ��Ϊ`AccessFlags.InstancePublic`��
���˹���ʵ���Ŀɷ�����ע�룬�����г��������ɷ����Ե�Attribute���������Ŀɷ����Ի���������Ҫ�󣬿��Ը����Լ��������Լ��̳�`OtelAttribute`��`PureOtelAttribute`��
Ȼ����д`Flags`����

|��С�汾Ҫ��|Ĭ�ϼ�¼�����ͷ���ֵ|Ĭ�ϲ���¼�����ͷ���ֵ|�ɷ�����|
|:--:|:--:|:--:|:--|
|0.1.2|PublicOtelAttribute|PublicPureOtelAttribute|����public�����������Ǿ�̬��������ʵ������|
|0.1.2|StaticOtelAttribute|StaticPureOtelAttribute|����public static����|
|0.1.2|FullOtelAttribute|FullPureOtelAttribute|���з��������ۿɷ��������|

## ͨ������Attribute֯��
[����Attribute֯��](https://github.com/inversionhourglass/Rougamo#attribute%E4%BB%A3%E7%90%86%E7%BB%87%E5%85%A5moproxyattribute)ͬ����`Rougamo`�ṩ��֯�뷽ʽ��
����һ��ʹ�ó����ǽ��[Rougamo.APM.Abstractions](https://github.com/inversionhourglass/Rougamo.APM#rougamoapmabstractions)ʹ�ã�����Attributeʹ��`Rougamo.APM.Abstractions`
�ṩ��`SpanAttribute`��`PureSpanAttribute`��������Attribute������֯����룬�������ڱ�ǣ�֮����ͨ������ķ�ʽ����������֯��
```csharp
// Startup.cs����AssemblyInfo.cs����Ŀ��������λ���������ĳ��򼯼����Attribute
[assembly: MoProxy(typeof(SpanAttribute), typeof(OtelAttribute))]
[assembly: MoProxy(typeof(PureSpanAttribute), typeof(PureOtelAttribute))]

public class Cls
{
    [Span]  // ����ͨ������֯��OtelAttribute�Ĵ���ʵ��
    public int M1()
    {
        // ...
        return 123;
    }

    [PureSpan]  // ����ͨ������֯��PureOtelAttribute�Ĵ���ʵ��
    private async Task<string> M2Async()
    {
        // ...
        return string.Empty;
    }
}
```
ͨ������ķ�ʽ�޷�ʹ�ýӿ�֯�룬Ҳ�޷��Զ��������OtelAttribute.Name������ʹ�����ַ�ʽ���������ڣ�����㻹��ȷ�����APM����ʹ��ʲô���߿��ܻỻ����`OpenTelemetry`��֧�ֵ�APM��
��ô��ʹ�����ִ���ķ�ʽ�Ϳ��Խ����޸�����ʵ��������`MoProxyAttribute`ָ���������͵Ĵ��롣

### todo
- English document