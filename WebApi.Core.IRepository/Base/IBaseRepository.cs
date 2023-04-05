namespace WebApi.Core.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> QueryById(object id);

        Task<List<TEntity>> Query();

        Task<bool> Create(TEntity entity);

        Task<bool> Update(TEntity entity);

        Task<bool> DeleteByIds(object[] ids);
    }
}
