using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Reflection;
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
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (c, p) =>
                    {
                        if (p.IsPrimarykey == false && new NullabilityInfoContext().Create(c).WriteState is NullabilityState.Nullable)
                        {

                        }
                    }
                }
            });

            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)) + "\r\n");
            };
        }

        public void InitSchemaTable()
        {
            Type[] types = Assembly.LoadFrom("WebApi.Core.Model.dll")
                .GetTypes()
                .Where(item => item.FullName.Contains("WebApi.Core.Model"))
                .ToArray();

            foreach (var table in types)
            {
                Console.WriteLine($"Start init datatable {table.FullName}");
                Db.CodeFirst.SetStringDefaultLength(types.Length).InitTables(table);
                Console.WriteLine($"End init datatable {table.FullName}");
            }
        }
    }
}
