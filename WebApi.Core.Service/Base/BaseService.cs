using WebApi.Core.IService.Base;
using WebApi.Core.Repository.Base;

namespace WebApi.Core.Service.Base;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
{
    public IBaseRepository<TEntity> BaseDal { get; set; }
    public BaseService(IBaseRepository<TEntity> baseDal)
    {
        BaseDal = baseDal;
    }
}
