using WebApi.Core.IRepository;
using WebApi.Core.Model;
using WebApi.Core.Repository.Base;

namespace WebApi.Core.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
    }
}
