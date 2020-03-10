using Framework.Core.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.CodeTemplate
{
    public class CodeContext : ICodeContext
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public CodeContext(ISqlSugarClient sqlSugarClient)
        {
            this._sqlSugarClient = sqlSugarClient;
        }


        public async Task<List<TableInfo>> MysqlGetTablesAsync()
        {
            return (await _sqlSugarClient.SqlQueryable<MysqlTableInfo>($"SELECT * FROM information_schema.TABLES  WHERE table_schema='{DBConfig.DbName}'")
                .ToListAsync())
                .Select(p => new TableInfo()
                {
                    Name = p.TABLE_NAME,
                    CommentsValue = p.TABLE_MOMMENT,
                    RowsCount = p.TABLE_ROWS,
                    CreateTime = p.CREATE_TIME,
                    UpdateTime = p.UPDATE_TIME,
                    encode = p.TABLE_COLLATION
                }).ToList();
        }

        public async Task<List<TableFieldInfo>> MysqlGetTableFieldAsync(string TableName)
        {
            return (await _sqlSugarClient.SqlQueryable<MysqlTableFieldInfo>($"select COLUMN_NAME,COLUMN_COMMENT,DATA_TYPE,IS_NULLABLE from information_schema.COLUMNS where table_schema='{DBConfig.DbName}' and  TABLE_NAME='{TableName}'")
                 .ToListAsync()).Select(p => new TableFieldInfo()
                 {
                     ColDbType = p.DATA_TYPE,
                     IsNullAble = p.IS_NULLABLE.ToUpper() == "YES",
                     CommentsValue = p.COLUMN_COMMENT,
                     ColName = p.COLUMN_NAME,
                     TableName = TableName,
                     ColType = ConvertToDBType.MysqlChangeDBTypeToCSharpType(p.DATA_TYPE)
                 }).ToList();
        }
    }


    #region 实体模型

    public class MysqlTableInfo
    {
        public string TABLE_NAME { get; set; } //表名称
        public string TABLE_MOMMENT { get; set; } //表注释
        public string TABLE_ROWS { get; set; } //表行数
        public string CREATE_TIME { get; set; } //创建时间
        public string UPDATE_TIME { get; set; } //更新时间
        public string TABLE_COLLATION { get; set; } //编码格式
    }

    public class TableInfo
    {
        public string Name { get; set; } //表名称
        public string CommentsValue { get; set; } //表注释

        public string RowsCount { get; set; } //表行数

        public string CreateTime { get; set; } //创建时间

        public string UpdateTime { get; set; } //更新时间

        public string encode { get; set; }//编码格式

        public bool hasChildren { get; set; } = true;  //视图用
    }

    public class MysqlTableFieldInfo
    {
        public string COLUMN_NAME { get; set; }  //字段名称
        public string COLUMN_COMMENT { get; set; }  //字段注释
        public string DATA_TYPE { get; set; } //字段类型
        public string IS_NULLABLE { get; set; } //NO不为空 YSE为空
    }

    public class TableFieldInfo
    {
        public string ColDbType { get; set; } //数据库对应字段类型
        public string ColName { get; set; }  //列名
        public string ColType { get; set; }  //代码对应类型
        public bool IsNullAble { get; set; } //是否为空
        public string CommentsValue { get; set; } //字段注释
        public string TableName { get; set; } //表名
    }


    #endregion
}
