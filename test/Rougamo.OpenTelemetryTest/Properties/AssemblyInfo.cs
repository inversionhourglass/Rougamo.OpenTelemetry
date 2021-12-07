using Rougamo;
using Rougamo.APM;
using Rougamo.OpenTelemetry;

[assembly: MoProxy(typeof(SpanAttribute), typeof(OtelAttribute))]