using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Common.Global;
using WebApi.Core.IService;
using WebApi.Core.Model;
using WebApi.Core.Service;

namespace WebApi.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        public UserController(IUserService user)
        {
            _user = user;
        }

        [HttpPost]
        public async Task<MessageModel<bool>> CreateUser([FromForm] User model)
        {
            return await _user.Create(model);
        }

        [HttpGet]
        public async Task<MessageModel<List<User>>> GetAllUser()
        {
            return await _user.GetAllUsers();
        }

        [HttpGet]
        public async Task<MessageModel<string>> Login(string userId, string userName)
        {
            return await _user.Login(userId, userName);
        }
    }
}
