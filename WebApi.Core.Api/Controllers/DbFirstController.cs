using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using WebApi.Core.Common.DB;
using WebApi.Core.Common.Global;
using WebApi.Core.IService;
using WebApi.Core.Model.Models;

namespace WebApi.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DbFirstController : ControllerBase
    {
        private readonly ITestService _test;
        private readonly IWebHostEnvironment Env;
        private readonly SqlSugarScope _client;
        public DbFirstController(ISqlSugarClient client, ITestService test, IWebHostEnvironment env)
        {
            Env = env;
            _test = test;
            _client = client as SqlSugarScope;
        }

        [HttpPost]
        public async Task<Test> CreateNew()
        {
            return await _test.CreateTest();
        }

        [HttpPost]
        public MessageModel<string> CreateFileByDBEntities([FromBody] string[] tableNames, [FromQuery] string ConnId = "WebApi")
        {
            ConnId ??= CustomContext.connId;
            string webRootPath = Env.ContentRootPath;
            string msg = "";
            if (Env.IsDevelopment())
            {
                msg += $"库{ConnId}-Model层生成：{FrameSeed.CreateModels(_client, ConnId, false, tableNames, webRootPath)} || ";
                //msg += $"库{ConnId}-IRepositorys层生成：{FrameSeed.CreateIRepositorys(_client, ConnId, false, tableNames, webRootPath)} || ";
                msg += $"库{ConnId}-IServices层生成：{FrameSeed.CreateIServices(_client, ConnId, false, tableNames, webRootPath)} || ";
                msg += $"库{ConnId}-Repository层生成：{FrameSeed.CreateRepository(_client, ConnId, false, tableNames, webRootPath)} || ";
                msg += $"库{ConnId}-Services层生成：{FrameSeed.CreateServices(_client, ConnId, false, tableNames, webRootPath)} || ";
                msg += $"Controller层生成：{FrameSeed.CreateControllers(_client, ConnId, false, tableNames, webRootPath)} || ";
            }



            return MessageModel<string>.Success("", msg);
        }
    }
}
