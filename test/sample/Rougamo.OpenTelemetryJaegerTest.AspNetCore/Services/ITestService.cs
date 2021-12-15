using System.Threading.Tasks;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services
{
    public interface ITestService : IService
    {
        int RandomTimeout(string seed);

        void Exception();

        void BackgroundComplete();

        void BackgroundTask();
    }
}
