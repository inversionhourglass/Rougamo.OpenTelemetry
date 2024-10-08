﻿using Rougamo.APM;
using Rougamo.OpenTelemetry;
using Rougamo.OpenTelemetryJaegerTest.AspNetCore.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        public void BackgroundComplete()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DelayAsync(5000);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void BackgroundTask()
        {
            Task.Run(() => Delay(5000));
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

        [Otel]
        private async Task DelayAsync(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }

        [Otel]
        private void Delay(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}
