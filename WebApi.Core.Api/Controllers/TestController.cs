using Microsoft.AspNetCore.Mvc;
using WebApi.Core.IService;
using WebApi.Core.Model;
using WebApi.Core.Service;

namespace WebApi.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _test;
        public TestController(ITestService test)
        {
            _test = test;
        }

        [HttpPost]
        public async Task<Test> CreateNew()
        {
            return await _test.CreateTest();
        }
    }
}
