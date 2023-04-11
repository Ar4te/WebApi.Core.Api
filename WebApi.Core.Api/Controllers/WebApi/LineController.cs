using WebApi.Core.IService;
using WebApi.Core.Model;
using WebApi.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Core.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
     public class LineController : ControllerBase
    {
            /// <summary>
            /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
            /// </summary>
            private readonly ILineService _lineServices;
    
            public LineController(ILineService LineServices)
            {
                _lineServices = LineServices;
            }
    }
}