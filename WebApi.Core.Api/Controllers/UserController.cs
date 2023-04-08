using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Common.Global;
using WebApi.Core.IService;
using WebApi.Core.Model;
using WebApi.Core.Service;
using WebApi.Core.ViewModel;

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
        public async Task<MessageModel<bool>> CreateUser([FromForm] UserVM model)
        {
            return await _user.Create(model);
        }

        [HttpGet]
        public async Task<MessageModel<List<User>>> GetAllUser()
        {
            return await _user.GetAllUsers();
        }

        [HttpGet]
        public async Task<MessageModel<string>> Login(string userName, string password)
        {
            return await _user.Login(userName, password);
        }
    }
}
