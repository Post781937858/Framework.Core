using Framework.Core.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Framework.Core.Common
{
    public class DBClientManage
    {
        public static ISqlSugarClient GetSqlSugarClient(InitKeyType keyType = InitKeyType.SystemTable)
        {
            return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
            {
                ConnectionString = DBConfig.ConnectionString,//必填, 数据库连接字符串
                DbType = (SqlSugar.DbType)DBConfig.DbType,//必填, 数据库类型
                IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = keyType //默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
            });
        }
    }
}
