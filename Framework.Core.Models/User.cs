using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User: RootEntity
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        [SugarColumn(Length = 60, IsNullable = false, ColumnDataType = "nvarchar")]
        public string UserNumber { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string Password { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Remark { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string PowerName { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int UserState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
        public string showName { get; set; }

        /// <summary>
        ///图像 
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Imgurl { get; set; }
    }
}
