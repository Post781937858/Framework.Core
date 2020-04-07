using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "友链模型")]
    public class BlogLinks : RootEntity
    {
        /// <summary>
        /// 链接名称
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = false, ColumnDataType = "nvarchar", ColumnDescription = "链接名称")]
        public string LinkName { get; set; }

        /// <summary>
        /// 链接Url
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = false, ColumnDataType = "nvarchar", ColumnDescription = "链接Url")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 链接状态
        /// </summary>
        [SugarColumn( ColumnDescription = "链接状态")]
        public int Status { get; set; }
    }
}
