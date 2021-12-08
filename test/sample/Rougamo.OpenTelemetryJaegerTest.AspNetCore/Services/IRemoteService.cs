using System.Threading.Tasks;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services
{
    public interface IRemoteService : IService
    {
        Task<string> LocalPostAsync(int timeout);
    }
}
