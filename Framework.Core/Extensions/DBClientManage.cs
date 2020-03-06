using Framework.Core.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Models;
using System.IO;
using System.Reflection;

namespace Framework.Core
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

        public static async Task SeedAsync()
        {
            ISqlSugarClient sugarClient = GetSqlSugarClient(InitKeyType.Attribute);
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
            if (!await sugarClient.Queryable<Menu>().AnyAsync())
            {
                List<Menu> menulist = new List<Menu>();
                menulist.Add(new Menu()
                {
                    title = "系统首页",
                    menuid = 999,
                    icon = "el-icon-s-home",
                    menutype = menuType.Menu,
                    url = "#",
                    state = 200,
                    no = 0
                });
                menulist.Add(new Menu()
                {
                    title = "权限管理",
                    menuid = 999,
                    icon = "el-icon-s-custom",
                    menutype = menuType.Menu,
                    url = "#",
                    state = 200,
                    no = 0
                });
                menulist.Add(new Menu()
                {
                    title = "控制台",
                    menuid = 1,
                    icon = "",
                    menutype = menuType.Menu,
                    url = "/HomeMain",
                    state = 200,
                    no = 0
                });
                menulist.Add(new Menu()
                {
                    title = "用户管理",
                    menuid = 2,
                    icon = "",
                    menutype = menuType.Menu,
                    url = "/Power/Role",
                    state = 200,
                    no = 0
                });


                new SimpleClient<Menu>(sugarClient).InsertRange(menulist);
            }
            if (!await sugarClient.Queryable<User>().AnyAsync())
            {
                List<User> Userlist = new List<User>();
                Userlist.Add(new User()
                {
                    CreateTime = DateTime.Now,
                    UserNumber = "123456",
                    Password = "123456",
                    PowerName = "admin",
                    showName = "贤心",
                    UserState = 200
                });
                new SimpleClient<User>(sugarClient).InsertRange(Userlist);
            }
            if (!await sugarClient.Queryable<PowerGroup>().AnyAsync())
            {
                List<PowerGroup> PowerGrouplist = new List<PowerGroup>();
                PowerGrouplist.Add(new PowerGroup()
                {
                    name = "admin",
                    explain = "超级管理员",
                    CreateTime = DateTime.Now,
                    CreateUserId = 1
                });
                PowerGrouplist.Add(new PowerGroup()
                {
                    name = "system",
                    explain = "系统管理员",
                    CreateTime = DateTime.Now,
                    CreateUserId = 1
                });
                PowerGrouplist.Add(new PowerGroup()
                {
                    name = "root",
                    explain = "超级管理员",
                    CreateTime = DateTime.Now,
                    CreateUserId = 1
                });
                new SimpleClient<PowerGroup>(sugarClient).InsertRange(PowerGrouplist);
            }
            if (!await sugarClient.Queryable<PowerDetail>().AnyAsync())
            {
                List<PowerDetail> PowerDetaillist = new List<PowerDetail>();
                for (int i = 1; i < 300; i++)
                {
                    PowerDetaillist.Add(new PowerDetail()
                    {
                        PowerName = "admin",
                        menuid = i,
                    });
                }
                new SimpleClient<PowerDetail>(sugarClient).InsertRange(PowerDetaillist);
            }
        }
    }
}
