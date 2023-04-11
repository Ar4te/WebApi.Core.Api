using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Collections.Concurrent;

namespace WebApi.Core.Repository.UnitOfWork
{
    public class UnitOfWorkManage : IUnitOfWorkManage
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;
        private string dbId = "";
        private int _tranCount { get; set; }
        public int TranCount => _tranCount;
        public readonly ConcurrentStack<string> TranStack = new();
        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            _tranCount = 0;
        }

        public SqlSugarScope GetDbClient()
        {
            return _sqlSugarClient as SqlSugarScope;
        }

        public void BeginTran()
        {
            lock (this)
            {
                _tranCount++;
                GetDbClient().BeginTran();
            }
        }

        public void CommitTran()
        {
            lock (this)
            {
                _tranCount--;
                if (_tranCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                    }
                }
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                _tranCount--;
                GetDbClient().RollbackTran();
            }
        }

        public UnitOfWork CreateUnitOfWork()
        {
            UnitOfWork uow = new();
            uow.logger = _logger;
            uow.Db = _sqlSugarClient;
            uow.Tenant = (ITenant)_sqlSugarClient;
            uow.IsTran = true;

            uow.Db.Open();
            uow.Tenant.BeginTran();
            _logger.LogDebug("UnitOfWork Begin");
            return uow;
        }

        public IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class, new()
        {
            dbId = "";
            return GetDbClient().SqlQueryable<T>(sql).ToList().AsQueryable();
        }

        public IQueryable<T> FromSqlFromDbId<T>(string sql, string _dbId, params object[] parameters) where T : class, new()
        {
            dbId = _dbId;
            return GetDbClient().SqlQueryable<T>(sql).ToList().AsQueryable();
        }
    }
}
