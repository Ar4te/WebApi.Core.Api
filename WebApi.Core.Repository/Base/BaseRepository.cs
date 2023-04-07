using SqlSugar;
using WebApi.Core.Common.Global;
using WebApi.Core.IRepository.Base;
using WebApi.Core.Repository.SqlSugar;

namespace WebApi.Core.Repository.Base
{
    public class BaseRepository<TEntity> : DbContext<TEntity>, IBaseRepository<TEntity> where TEntity : class, new()
    {
        #region Create
        /// <summary>
        /// 返回是否插入成功
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> Create(TEntity entity)
        {
            var res = await Db.Insertable(entity).ExecuteCommandAsync();
            return res > 0;
        }

        public async Task<TEntity> CreateNew(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnEntityAsync();
        }

        public async Task<int> Create(List<TEntity> entities)
        {
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }
        #endregion

        #region Query
        public async Task<List<TEntity>> Query()
        {
            var res = await Db.Queryable<TEntity>().ToListAsync();
            return res;
        }

        public async Task<TEntity> QueryById(object id)
        {
            return await Db.Queryable<TEntity>().InSingleAsync(id);
        }

        /// <summary>
        /// 执行Sql语句查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// Sql语句分页查询
        /// </summary>
        /// <param name="querySqlStr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="countSqlStr"></param>
        /// <param name="strOrderByField"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<PageModel<TEntity>> QuerySqlPage(string querySqlStr, int pageIndex, int pageSize, string countSqlStr = "", string strOrderByField = "", SugarParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(countSqlStr))
                countSqlStr = $"SELECT COUNT(*) FROM ({querySqlStr}) A";

            if (!string.IsNullOrEmpty(strOrderByField))
                querySqlStr += $" Order By {strOrderByField}";

            querySqlStr += $" LIMIT {pageIndex - 1},{pageSize}";
            var data = await Db.Ado.SqlQueryAsync<TEntity>(querySqlStr, parameters);
            var dataCount = await Db.Ado.SqlQuerySingleAsync<int>(countSqlStr);
            return new PageModel<TEntity>(pageIndex, pageSize, dataCount, data);
        }

        #endregion

        #region Update
        public async Task<bool> Update(TEntity entity)
        {
            var res = await Db.Updateable(entity).ExecuteCommandAsync();
            return res > 0;
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteByIds(object[] ids)
        {
            var res = await Db.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
            return res > 0;
        }
        #endregion

        #region NoGroup
        /// <summary>
        /// 执行增删改SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSql(string sql)
        {
            return await Db.Ado.ExecuteCommandAsync(sql);
        }

        /// <summary>
        /// 将实体从A表转移到B表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> InsertA2B<T>(List<T> entity, string tableName) where T : class, new()
        {
            var insert = Db.Insertable(entity).AS(tableName);

            return await insert.ExecuteCommandAsync();
        }
        #endregion
    }
}
