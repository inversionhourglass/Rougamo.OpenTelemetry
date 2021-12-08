using Rougamo.APM;
using Rougamo.OpenTelemetry;
using Rougamo.OpenTelemetryJaegerTest.AspNetCore.Utils;
using System;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services.Impl
{
    public class TestService : ITestService
    {
        public int RandomTimeout(string seed)
        {
            return RandomUtils.RandomMod(seed) % 30000;
        }

        public void Exception()
        {
            Announce();
        }

        [ApmExceptionAnnounce] // Throw have been recorded exception, this attribute will record exception again
        [Otel] // IRougamo<OtelAttribute> will not weave private method by default
        private void Announce()
        {
            Throw();
        }

        [Otel]
        private void Throw()
        {
            throw new System.NotImplementedException();
        }
    }
}
