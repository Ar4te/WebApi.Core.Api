using WebApi.Core.Common.Global;
using WebApi.Core.IService.Base;
using WebApi.Core.Model.Models;
using WebApi.Core.Model.ViewModels;

namespace WebApi.Core.IService
{
    public interface IUserService : IBaseService<User>
    {
        Task<MessageModel<bool>> Create(UserVM user);
        Task<MessageModel<List<User>>> GetAllUsers();
        Task<MessageModel<string>> Login(string userName, string password);
    }
}
