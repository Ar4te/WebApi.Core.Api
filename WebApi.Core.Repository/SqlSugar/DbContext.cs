using SqlSugar;
using WebApi.Core.Common.Helper;
using WebApi.Core.Model;

namespace WebApi.Core.Repository.SqlSugar
{
    public class DbContext<T> where T : class, new()
    {
        public SqlSugarScope Db;
        public SimpleClient<T> CurrentDb
        {
            get
            {
                return new SimpleClient<T>(Db);
            }
        }
        public SimpleClient<User> UserDb
        {
            get
            {
                return new SimpleClient<User>(Db);
            }
        }

        public DbContext()
        {
            Db = new SqlSugarScope(new ConnectionConfig
            {
                ConnectionString = AppSettings.app("DBS", "ConnectionString"),
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
            });

            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)) + "\r\n");
            };
        }
    }
}
