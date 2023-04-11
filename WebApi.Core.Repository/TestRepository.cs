using WebApi.Core.Model.Models;
using WebApi.Core.Repository.Base;
using WebApi.Core.Repository.UnitOfWork;

namespace WebApi.Core.Repository
{
    public class TestRepository : BaseRepository<Test>
    {
        public TestRepository(IUnitOfWorkManage unitOfWork) : base(unitOfWork)
        {
        }
    }
}
