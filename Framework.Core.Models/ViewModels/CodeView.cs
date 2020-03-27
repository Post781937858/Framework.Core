using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Framework.Core.Models.ViewModels
{
    public class CodeView
    {

        public string VueOutputPath { get; set; }

        public string ControllersOutputPath { get; set; }

        public string ModelOutputPath { get; set; }

        public string IServicesOutputPath { get; set; }

        public string IRepositoryOutputPath { get; set; }

        public string ServicesOutputPath { get; set; }

        public string RepositoryOutputPath { get; set; }

        public string VueCode { get; set; }

        public bool IsPaging { get; set; }

        public bool tableToolOn { get; set; }

        public List<modelProperty> Propertys { get; set; }

        public modelInfo model { get; set; }

        public int menuId { get; set; }

        public string icon { get; set; }

        public string url { get; set; }

        public int sort { get; set; }

        public string description { get; set; }

        public string title { get; set; }
    }

    public class resultCode
    {

        public string ModelCode { get; set; }

        public string IServicesCode { get; set; }

        public string IRepositoryCode { get; set; }

        public string ServicesCode { get; set; }

        public string RepositoryCode { get; set; }

        public string controllerCode { get; set; }

        public string VueCode { get; set; }

    }

    #region 实体模型


    public class modelInfo
    {
        public string modelName { get; set; }

        public string Description { get; set; }
    }

    public class modelPropertyView
    {
        public string ColumnName { get; set; }

        public string ColumnType { get; set; }

        public string ColumnDescription { get; set; }
    }

    public class modelProperty : modelPropertyView, IMapperTo<modelPropertyView>
    {
        public PropertyInfo propertyInfo { get; set; }
    }


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

        public string Brief { get; set; } //表注释

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
        public string Brief { get; set; } //字段注释
        public string TableName { get; set; } //表名
    }


    #endregion
}
