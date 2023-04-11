using WebApi.Core.IRepository;
using WebApi.Core.Model;
using WebApi.Core.Repository.Base;
using WebApi.Core.Repository.UnitOfWork;

namespace WebApi.Core.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IUnitOfWorkManage unitOfWork) : base(unitOfWork)
        {
        }
    }
}
