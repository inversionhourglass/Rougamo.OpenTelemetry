using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Rougamo.APM;
using Rougamo.OpenTelemetry;

namespace Rougamo.OpenTelemetryTest
{
    class Program
    {
        private readonly static Random _Random = new Random();

        public static void Main()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(options => options
                    .AddConsoleExporter());
            });

            var logger = loggerFactory.CreateLogger<Program>();

            OtelSingleton.Options = new OtelRougamoOptions
            {
                ArgumentsStoreType = ArgumentsStoreType.Tag
            };

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddRougamoSource()
                .AddJaegerExporter()
                .AddConsoleExporter()
                .Build();

            RandomMod(Guid.NewGuid());

            Console.ReadLine();
        }

        [return: ApmIgnore]
        [Span]
        public static int Random()
        {
            return _Random.Next();
        }

        [Otel]
        public static int Hash([ApmIgnore]object obj)
        {
            return obj.GetHashCode();
        }

        [Otel(Name = "Randomod", RecordArguments =false)]
        public static int RandomMod(Guid guid)
        {
            var r1 = Random();
            var r2 = Hash(guid);
            return Mod(r1, r2);
        }

        [Span]
        public static int Mod(int r1, int r2)
        {
            return r1 % r2;
        }
    }
}
