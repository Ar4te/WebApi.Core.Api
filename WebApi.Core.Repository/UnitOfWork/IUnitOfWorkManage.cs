using SqlSugar;

namespace WebApi.Core.Repository.UnitOfWork
{
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();
        int TranCount { get; }
        UnitOfWork CreateUnitOfWork();
        void BeginTran();
        void CommitTran();
        void RollbackTran();

        IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class, new();
        IQueryable<T> FromSqlFromDbId<T>(string sql, string dbId, params object[] parameters) where T : class, new();
    }
}
