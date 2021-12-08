using Rougamo.OpenTelemetry;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services
{
    public interface IService : IRougamo<OtelAttribute>
    {
    }
}
