using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SqlSugar;
using WebApi.Core.Common.Helper;

namespace WebApi.Core.Common.Extension;

public static class SqlSugarExt
{
    public static void AddSqlSugarSetup(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddSingleton<ISqlSugarClient>(options =>
        {
            var connCfg = new ConnectionConfig
            {
                ConfigId = AppSettings.app("DBS", "ConfigId"),
                ConnectionString = AppSettings.app("DBS", "ConnectionString"),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        if (AppSettings.app("SqlAOP", "Enabled").ToUpper() == "TRUE")
                        {
                            Parallel.For(0, 1, e =>
                            {
                                Log.Information("SqlLog", new string[] { sql.GetType().ToString(), GetParas(p), "【SQL语句】：" + sql });
                            });
                        }

                        if (AppSettings.app("SqlAOP", "LogToConsole").ToUpper() == "TRUE")
                        {
                            Console.WriteLine(string.Join("\r\n", new string[] { "--------", $"{DateTime.Now:yyyyMMdd:HHmmss} : " + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                        }
                    }
                },
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true,
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (property, column) =>
                    {
                        if (column.IsPrimarykey && column.IsIdentity && property.PropertyType == typeof(int))
                        {
                            column.IsIdentity = true;
                        }
                    }
                },
                InitKeyType = InitKeyType.Attribute
            };

            return new SqlSugarScope(connCfg);
        });
    }
    public static string GetWholeSql(SugarParameter[] pars, string sql)
    {
        foreach (var param in pars)
        {
            sql.Replace(param.ParameterName, param.Value.ToString());
        }
        return sql;
    }
    public static string GetParas(SugarParameter[] pars)
    {
        string key = "【SQL参数】：";
        foreach (var param in pars)
        {
            key += $"{param.ParameterName}:{param.Value}\n";
        }
        return key;
    }
}
