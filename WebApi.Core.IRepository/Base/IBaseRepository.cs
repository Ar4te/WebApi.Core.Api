using SqlSugar;

namespace WebApi.Core.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        #region Create
        /// <summary>
        /// 返回是否插入成功
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Create(TEntity entity);

        /// <summary>
        /// 返回插入后的完整实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> CreateNew(TEntity entity);

        /// <summary>
        /// 批量插入，返回受影响的行数
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> Create(List<TEntity> entities);
        #endregion

        #region Query
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> QueryById(object id);

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> Query();

        /// <summary>
        /// 原生SQL查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null);

        #endregion

        #region Update
        Task<bool> Update(TEntity entity);

        #endregion

        #region Delete
        Task<bool> DeleteByIds(object[] ids);

        #endregion

        #region NoGroup
        /// <summary>
        /// 同时实体从A转移到B
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> InsertA2B<TEntity>(List<TEntity> entity, string tableName) where TEntity : class, new();

        /// <summary>
        /// 执行增删改SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteSql(string sql);
        #endregion
    }
}
