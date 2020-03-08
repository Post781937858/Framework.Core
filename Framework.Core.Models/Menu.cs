using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    /// <summary>
    ///功能实体
    /// </summary>
    public class Menu : RootEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
        public string title { get; set; }

        /// <summary>
        /// icon图标
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string icon { get; set; }

        /// <summary>
        /// url地址
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
        public string url { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int state { get; set; }


        /// <summary>
        /// 菜单类型
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public menuType menutype { get; set; }

        /// <summary>
        /// API接口类型 GET POST Put Delete
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string method { get; set; }

        /// <summary>
        /// 子菜单ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int menuid { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string explain { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int no { get; set; }

    }

    public enum menuType
    {
        Menu,

        Button,

        Api,

        ALL = 99
    }
}
