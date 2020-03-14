using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "角色模型")]
    public class PowerGroup: RootEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
        public string name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
        public string explain { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public DateTime CreateTime { get; set; }
    }
}
