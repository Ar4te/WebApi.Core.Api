using WebApi.Core.Common.Global;
using WebApi.Core.Common.Helper;
using WebApi.Core.IRepository.Base;
using WebApi.Core.IService;
using WebApi.Core.Model;
using WebApi.Core.Service.Base;

namespace WebApi.Core.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IBaseRepository<User> _baseDal;
        public UserService(IBaseRepository<User> baseDal) : base(baseDal)
        {
            _baseDal = baseDal;
        }
        public async Task<MessageModel<bool>> Create(User entity)
        {
            var res = await _baseDal.Create(entity);
            return res ? MessageModel<bool>.Success("操作成功", res) : MessageModel<bool>.Fail("操作失败");
        }

        public async Task<MessageModel<List<User>>> GetAllUsers()
        {
            var res = await _baseDal.Query();
            return res != null ? MessageModel<List<User>>.Success("查询成功", res) : MessageModel<List<User>>.Success("系统异常");
        }

        public async Task<MessageModel<string>> Login(string userId, string userName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName)) return MessageModel<string>.Fail("传入参数有误");

            var jwtStr = (new JwtHelper(userId, userName)).IssueJwt();

            return string.IsNullOrEmpty(jwtStr) ? MessageModel<string>.Fail("获取token失败") : MessageModel<string>.Success("", jwtStr);
        }
    }
}
