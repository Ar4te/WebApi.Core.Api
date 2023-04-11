using WebApi.Core.IService.Base;
using WebApi.Core.Repository.Base;

namespace WebApi.Core.Service.Base;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
{
    private readonly IBaseRepository<TEntity> _baseDal;
    public BaseService(IBaseRepository<TEntity> baseDal)
    {
        _baseDal = baseDal;
    }
}
