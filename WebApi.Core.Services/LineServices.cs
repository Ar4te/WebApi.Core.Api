
using WebApi.Core.IService;
using WebApi.Core.Model.Models;
using WebApi.Core.Services.BASE;
using WebApi.Core.IRepository.Base;

namespace WebApi.Core.Services
{
    public class LineServices : BaseServices<Line>, ILineServices
    {
        private readonly IBaseRepository<Line> _dal;
        public LineServices(IBaseRepository<Line> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}