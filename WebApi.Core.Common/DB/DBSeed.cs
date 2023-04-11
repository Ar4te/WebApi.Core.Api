using Serilog;
using SqlSugar;
using System.Reflection;
using WebApi.Core.Common.Helper;

namespace WebApi.Core.Common.DB;

public class DBSeed
{
    public static async Task SeedAsync(CustomContext cCtx, string webRootPath)
    {
        try
        {
            // 创建数据库
            Log.Information($"Create DataBase (The Db Id: {CustomContext.connId})\r\n");
            Console.WriteLine($"Create DataBase (The Db Id: {CustomContext.connId})\r\n");
            var splitTableInfos = AppSettings.app<SplitTableInfo>("SplitTable");
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "WebApi.Core.Model.dll").Select(Assembly.LoadFrom).ToArray();
            var dbStr = BaseDBConfig.ConnectionString;
            cCtx._db.ChangeDatabase(CustomContext.connId);
            var db = cCtx._db;
            if (db.CurrentConnectionConfig.DbType != (DbType)DataBaseType.Oracle)
            {
                db.DbMaintenance.CreateDatabase();
                Log.Information($"ConnId: {CustomContext.connId} Database created successfully!");
                Console.WriteLine($"ConnId: {CustomContext.connId} Database created successfully!");
            }
            else
            {
                //Oracle 数据库不支持该操作
                Console.WriteLine($"Oracle 数据库不支持该操作，可手动创建Oracle数据库!", ConsoleColor.Red);
            }

            // 创建数据库表
            string _namespace = "WebApi.Core.Model.Models";
            Console.WriteLine($"ConnId: {CustomContext.connId} Create Tables...", ConsoleColor.Yellow);
            var modelTypes = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x.IsClass && x.Namespace != null && x.Namespace.Equals(_namespace)).ToList();

            modelTypes.ForEach(t =>
            {
                if (!db.DbMaintenance.IsAnyTable(t.Name))
                {
                    db.Ado.IsEnableLogEvent = false;
                    if (t.CustomAttributes != null && t.CustomAttributes.Any())
                        db.CodeFirst.SplitTables().InitTables(t);
                    else
                        db.CodeFirst.InitTables(t);
                    Console.WriteLine($"ConnId: {CustomContext.connId} Table {t.Name} created successfully!\r\n", ConsoleColor.Green);
                }
            });
            Console.WriteLine($"ConnId: {CustomContext.connId} Tables created successfully!\r\n", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message + "\n" + ex.StackTrace);
        }
    }
}
