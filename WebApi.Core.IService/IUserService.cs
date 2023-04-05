using WebApi.Core.Common.Global;
using WebApi.Core.IRepository.Base;
using WebApi.Core.IService.Base;
using WebApi.Core.Model;

namespace WebApi.Core.IService
{
    public interface IUserService : IBaseService<User>
    {
        Task<MessageModel<bool>> Create(User user);
        Task<MessageModel<List<User>>> GetAllUsers();

        Task<MessageModel<string>> Login(string userId, string userName);
    }
}
