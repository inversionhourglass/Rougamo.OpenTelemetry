using Rougamo.APM;
using Rougamo.OpenTelemetry;
using System;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Utils
{
    public static class RandomUtils
    {
        private readonly static Random _Random = new Random();

        [return: ApmIgnore] // do not record return value
        [Span] // proxy to OtelAttribute through MoProxy which define in AssemblyInfo.cs
        public static int Random()
        {
            return _Random.Next();
        }

        [Otel]
        public static int Hash(object obj)
        {
            return obj == null ? (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) : obj.GetHashCode();
        }

        // custom span name and dot not record parameters and return value
        [Otel(Name = "Randomod", RecordArguments = false)]
        public static int RandomMod(string seed)
        {
            var r1 = Random();
            var r2 = Hash(new { guid = Guid.NewGuid(), seed });
            return Mod(r1, r2);
        }

        [return: ApmRecord] // record return value
        [PureSpan] // proxy to PureOtelAttribute through MoProxy which define in AssemblyInfo.cs
        private static int Mod(int r1, [ApmRecord] int r2)
        {
            return Math.Abs(r1) % Math.Abs(r2);
        }
    }
}
