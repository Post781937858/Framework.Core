using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using SqlSugar;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "错误日志模型")]
    public class ErrorLog : RootEntity
    {
        /// <summary>
        ///异常时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "异常时间")]
        public DateTime time { get; set; } = DateTime.Now;

        /// <summary>
        ///用户id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "用户id")]
        public int UserId { get; set; }

        /// <summary>
        ///用户名称
        /// </summary>
        [SugarColumn(Length = 60, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "用户名称")]
        public string UserName { get; set; }

        /// <summary>
        ///url
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "url")]
        public string url { get; set; }

        /// <summary>
        ///异常信息
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "异常信息")]
        public string errormsg { get; set; }

        /// <summary>
        /// 堆栈
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "堆栈")]
        public string errorstack { get; set; }
    }
}