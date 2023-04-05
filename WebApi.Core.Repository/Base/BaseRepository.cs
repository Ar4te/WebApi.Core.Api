using WebApi.Core.IRepository.Base;
using WebApi.Core.Repository.SqlSugar;

namespace WebApi.Core.Repository.Base
{
    public class BaseRepository<TEntity> : DbContext<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        public async Task<bool> Create(TEntity entity)
        {
            var res = await Db.Insertable(entity).ExecuteCommandAsync();
            return res > 0;
        }

        public async Task<bool> DeleteByIds(object[] ids)
        {
            var res = await Db.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
            return res > 0;
        }

        public async Task<List<TEntity>> Query()
        {
            var res = await Db.Queryable<TEntity>().ToListAsync();
            return res;
        }

        public async Task<TEntity> QueryById(object id)
        {
            return await Db.Queryable<TEntity>().InSingleAsync(id);
        }

        public async Task<bool> Update(TEntity entity)
        {
            var res = await Db.Updateable(entity).ExecuteCommandAsync();
            return res > 0;
        }
    }
}
