using Framework.Core.Common;
using Framework.Core.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Extensions
{
    public  class DBStartup
    {
        public static async Task SeedAsync()
        {
            ISqlSugarClient sugarClient = DBClientManage.GetSqlSugarClient(InitKeyType.Attribute);
            // 创建数据库
            sugarClient.DbMaintenance.CreateDatabase();
            //反射创建表 只需继承RootEntity即可
            typeof(RootEntity).Assembly.GetTypes()
                .Where(i => i.BaseType == typeof(RootEntity))
                .ToList().ForEach(item =>
                {
                    // 创建表
                    sugarClient.CodeFirst.InitTables(item);
                });
            await Task.FromResult("");
        }
    }
}
