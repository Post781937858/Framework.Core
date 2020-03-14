using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework.Core.Common
{
    public class DBConfig
    {
        private static string sqliteConnection = Appsettings.app(new string[] { "DB", "Sqlite", "SqliteConnection" });
        private static bool isSqliteEnabled = (Appsettings.app(new string[] { "DB", "Sqlite", "Enabled" })).ToBool();

        private static string sqlServerConnection = Appsettings.app(new string[] { "DB", "SqlServer", "SqlServerConnection" });
        private static bool isSqlServerEnabled = (Appsettings.app(new string[] { "DB", "SqlServer", "Enabled" })).ToBool();

        private static string mySqlConnection = Appsettings.app(new string[] { "DB", "MySql", "MySqlConnection" });
        private static bool isMySqlEnabled = (Appsettings.app(new string[] { "DB", "MySql", "Enabled" })).ToBool();

        public static DataBaseType DbType = DataBaseType.MySql;
        public static string ConnectionString
        {
            get
            {
                if (isSqliteEnabled)
                {
                    DbType = DataBaseType.Sqlite;
                    return $"DataSource=" + Path.Combine(Environment.CurrentDirectory, sqliteConnection);
                }
                else if (isSqlServerEnabled)
                {
                    DbType = DataBaseType.SqlServer;
                    return sqlServerConnection;
                }
                else if (isMySqlEnabled)
                {
                    DbType = DataBaseType.MySql;
                    return mySqlConnection;
                }
                else
                {
                    throw new Exception("DB配置错误请检查");
                }
            }
        }

        public static string DbName
        {
            get
            {
                return ConnectionString.Split(';').Where(p => p.ToLower().Contains("database")).FirstOrDefault().Split('=')[1];
            }
        }
    }

    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4
    }
}
