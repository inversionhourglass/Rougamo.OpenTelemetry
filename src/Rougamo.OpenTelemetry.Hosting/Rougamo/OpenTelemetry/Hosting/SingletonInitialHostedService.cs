using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Rougamo.APM.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rougamo.OpenTelemetry.Hosting
{
    class SingletonInitialHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;

        public SingletonInitialHostedService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var options = _provider.GetService(typeof(IOptions<OtelRougamoOptions>)) as IOptions<OtelRougamoOptions>;
            OtelSingleton.Options = options!.Value;

            var serializer = _provider.GetService(typeof(ISerializer)) as ISerializer;
            if (serializer != null)
            {
                OtelSingleton.Serializer = serializer;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
