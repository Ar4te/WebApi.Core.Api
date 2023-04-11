using SqlSugar;
using WebApi.Core.Common.Helper;

namespace WebApi.Core.Common.DB;
public class CustomContext
{
    private static string _connStr = AppSettings.app("DBS", "ConnectionString");
    private static DbType _dbType = (DbType)Convert.ToInt32(AppSettings.app("DBS", "DbType"));
    public static string connId = AppSettings.app("DBS", "ConfigId");
    public SqlSugarScope _db;

    public CustomContext(ISqlSugarClient sqlSugarClient)
    {
        _db = sqlSugarClient as SqlSugarScope;
    }

    public SimpleClient<T> GetEntityDB<T>() where T : class, new()
    {
        return new SimpleClient<T>(_db);
    }

    public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntities) where T : class, new()
    {
        Type[] lstTypes = null;
        if (lstEntities != null)
        {
            lstTypes = new Type[lstEntities.Length];
            for (int i = 0; i < lstEntities.Length; i++)
            {
                T t = lstEntities[i];
                lstTypes[i] = typeof(T);
            }
        }
        CreateTableByEntity(blnBackupTable, lstTypes);
    }

    public void CreateTableByEntity(bool blnBackupTable, params Type[] lstTypes)
    {
        if (blnBackupTable)
            _db.CodeFirst.BackupTable().InitTables(lstTypes);
        else
            _db.CodeFirst.InitTables(lstTypes);
    }

    public static void Init(string connStr, DbType dbType = DbType.MySql)
    {
        _connStr = connStr;
        _dbType = dbType;
    }

    public static ConnectionConfig GetConnectionConfig(bool blnIsAutoCloseConnection = true)
    {
        var cfg = new ConnectionConfig
        {
            ConnectionString = _connStr,
            DbType = _dbType,
            IsAutoCloseConnection = blnIsAutoCloseConnection,
        };
        return cfg;
    }

    public static SqlSugarScope GetCustomDB(ConnectionConfig cfg)
    {
        return new SqlSugarScope(cfg);
    }

    public static SimpleClient<T> GetCustomEntityDB<T>(SqlSugarScope sugarClient) where T : class, new()
    {
        return new SimpleClient<T>(sugarClient);
    }

    public static SimpleClient<T> GetCustomEntityDB<T>(ConnectionConfig cfg) where T : class, new()
    {
        SqlSugarScope client = GetCustomDB(cfg);
        return GetCustomEntityDB<T>(client);
    }
}
