using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    public class ErrorLog: RootEntity
    {
        /// <summary>
        ///异常时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime time { get; set; }

        /// <summary>
        ///用户id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int UserId { get; set; }

        /// <summary>
        ///用户名称
        /// </summary>
        [SugarColumn(Length = 60, IsNullable = true, ColumnDataType = "nvarchar")]
        public string UserName { get; set; }

        /// <summary>
        ///url
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string url { get; set; }

        /// <summary>
        ///异常信息
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string errormsg { get; set; }

        /// <summary>
        /// 堆栈
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string errorstack { get; set; }
    }
}
