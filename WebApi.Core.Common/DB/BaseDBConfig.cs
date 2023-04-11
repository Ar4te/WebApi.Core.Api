using WebApi.Core.Common.Helper;

namespace WebApi.Core.Common.DB;

public static class BaseDBConfig
{
    public static string ConnectionString = AppSettings.app("DBS", "ConnectionString");
}

public enum DataBaseType
{
    MySql = 0,
    SqlServer = 1,
    Sqlite = 2,
    Oracle = 3,
    PostgreSQL = 4,
    Dm = 5,
    Kdbndp = 6,
}
