using Microsoft.Extensions.Logging;
using WebApi.Core.Common.Global;
using WebApi.Core.Common.Helper;
using WebApi.Core.Repository.Base;
using WebApi.Core.IService;
using WebApi.Core.Model.Models;
using WebApi.Core.Model.ViewModels;
using WebApi.Core.Repository.UnitOfWork;
using WebApi.Core.Service.Base;

namespace WebApi.Core.Service;

public class UserService : BaseService<User>, IUserService
{
    private readonly IBaseRepository<User> _dal;
    private readonly ILogger<User> _log;
    private readonly IUnitOfWorkManage _uow;
    public UserService(IBaseRepository<User> baseDal, ILogger<User> log, IUnitOfWorkManage uow) : base(baseDal)
    {
        _dal = baseDal;
        _log = log;
        _uow = uow;
    }
    public async Task<MessageModel<bool>> Create(UserVM entity)
    {
        var _user = await _dal.Query(r => r.UserName == entity.UserName);
        if (_user != null && _user.Any()) return MessageModel<bool>.Fail("用户名已被使用");
        entity.Password = MD5Helper.MD5Encrypt32(entity.Password);
        _uow.BeginTran();
        var res = await _dal.Create(new User(entity));
        _uow.RollbackTran();
        return res ? MessageModel<bool>.Success("操作成功", res) : MessageModel<bool>.Fail("操作失败");
    }

    public async Task<MessageModel<List<User>>> GetAllUsers()
    {
        var res = await _dal.Query();
        return res != null ? MessageModel<List<User>>.Success("查询成功", res) : MessageModel<List<User>>.Success("系统异常");
    }

    public async Task<MessageModel<string>> Login(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return MessageModel<string>.Fail("传入参数有误");

        var _user = await _dal.Query(r => r.UserName == userName && r.Password == MD5Helper.MD5Encrypt32(password));
        if (_user is null || _user.Count <= 0) return MessageModel<string>.Fail("登陆失败，请检查用户名和密码");

        var jwtStr = JwtHelper.IssueJwt(_user[0].UserId.ToString(), userName);

        _log.LogInformation($"{DateTime.Now:yyyyMMdd:HHmmss}*****");

        return string.IsNullOrEmpty(jwtStr) ? MessageModel<string>.Fail("系统异常") : MessageModel<string>.Success("登陆成功", jwtStr);
    }
}
