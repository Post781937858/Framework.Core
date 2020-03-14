using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "操作日志模型")]
    public class operatingLog : RootEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription= "用户名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Operating { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime Date { get; set; }

        /// <summary>
        /// 详情说明
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Details { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string ip { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Browser { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
        public string OS { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int state { get; set; }
    }
}
