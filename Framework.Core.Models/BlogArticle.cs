using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    public class BlogArticle: RootEntity
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(Length = 60, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "创建人")]
        public string bsubmitter { get; set; }

        /// <summary>
        /// 标题blog
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "标题")]
        public string btitle { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "简介")]
        public string bcsynopsis { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "类别")]
        public string bcategory { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "内容")]
        public string bcontent { get; set; }


        /// <summary>
        /// 图片路径
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "图片路径")]
        public string bcimgsrc { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        [SugarColumn(ColumnDescription = "访问量")]
        public int btraffic { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        [SugarColumn(ColumnDescription = "评论数量")]
        public int bcommentNum { get; set; }

        /// <summary> 
        /// 修改时间
        /// </summary>
        [SugarColumn(ColumnDescription = "修改时间")]
        public DateTime bUpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public System.DateTime bCreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true, ColumnDataType = "nvarchar", ColumnDescription = "备注")]
        public string bRemark { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "逻辑删除")]
        public bool IsDeleted { get; set; }

    }
}
