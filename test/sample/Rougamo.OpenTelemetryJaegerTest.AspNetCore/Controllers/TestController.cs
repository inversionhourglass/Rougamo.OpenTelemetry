using Microsoft.AspNetCore.Mvc;
using Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services;
using System.Threading.Tasks;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly IRemoteService _remoteService;

        public TestController(ITestService testService, IRemoteService remoteService)
        {
            _testService = testService;
            _remoteService = remoteService;
        }

        [HttpGet]
        public async Task<string> Get(string seed)
        { 
            var timeout = _testService.RandomTimeout(seed);
            return "get -> " + await _remoteService.LocalPostAsync(timeout);
        }

        [HttpPost]
        public string Post()
        {
            return "post";
        }

        [HttpGet]
        [Route("[action]")]
        public void Exception()
        {
            _testService.Exception();
        }
    }
}
