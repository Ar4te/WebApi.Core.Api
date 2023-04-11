using SqlSugar;
using System.Linq.Expressions;
using WebApi.Core.Common.Global;
using WebApi.Core.Repository.UnitOfWork;

namespace WebApi.Core.Repository.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
{
    private ISqlSugarClient _db;
    private readonly IUnitOfWorkManage _unitOfWork;
    public BaseRepository(IUnitOfWorkManage unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _db = unitOfWork.GetDbClient();
    }

    #region Create
    /// <summary>
    /// 返回是否插入成功
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> Create(TEntity entity)
    {
        var res = await _db.Insertable(entity).ExecuteCommandAsync();
        return res > 0;
    }

    public async Task<TEntity> CreateNew(TEntity entity)
    {
        return await _db.Insertable(entity).ExecuteReturnEntityAsync();
    }

    public async Task<int> Create(List<TEntity> entities)
    {
        return await _db.Insertable(entities).ExecuteCommandAsync();
    }
    #endregion

    #region Query
    public async Task<List<TEntity>> Query()
    {
        var res = await _db.Queryable<TEntity>().ToListAsync();
        return res;
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> expression)
    {
        return await _db.Queryable<TEntity>().Where(expression).ToListAsync();
    }
    public async Task<TEntity> QueryById(object id)
    {
        return await _db.Queryable<TEntity>().InSingleAsync(id);
    }

    /// <summary>
    /// 执行Sql语句查询
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null)
    {
        return await _db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
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
        var data = await _db.Ado.SqlQueryAsync<TEntity>(querySqlStr, parameters);
        var dataCount = await _db.Ado.SqlQuerySingleAsync<int>(countSqlStr);
        return new PageModel<TEntity>(pageIndex, pageSize, dataCount, data);
    }

    #endregion

    #region Update
    public async Task<bool> Update(TEntity entity)
    {
        var res = await _db.Updateable(entity).ExecuteCommandAsync();
        return res > 0;
    }
    #endregion

    #region Delete
    public async Task<bool> DeleteByIds(object[] ids)
    {
        var res = await _db.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
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
        return await _db.Ado.ExecuteCommandAsync(sql);
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
        var insert = _db.Insertable(entity).AS(tableName);

        return await insert.ExecuteCommandAsync();
    }
    #endregion
}
