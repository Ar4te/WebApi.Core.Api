using WebApi.Core.Model.Models;
using WebApi.Core.Repository.Base;
using WebApi.Core.Repository.UnitOfWork;

namespace WebApi.Core.Repository;

public class LineRepository : BaseRepository<Line>
{
    public LineRepository(IUnitOfWorkManage unitOfWork) : base(unitOfWork)
    {
    }
}