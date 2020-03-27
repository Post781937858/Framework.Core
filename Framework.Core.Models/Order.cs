using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using SqlSugar;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "订单模型")]
    public  class Order : RootEntity
    {
        /// <summary>
        /// 消息id
        /// </summary>
        [SugarColumn(ColumnDescription = "消息id")]
        public int mesid { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [SugarColumn(ColumnDescription = "消息类型")]
        public string messageName { get; set; }

        /// <summary>
        /// 箱号ID
        /// </summary>
        [SugarColumn(ColumnDescription = "箱号ID")]
        public int boxId { get; set; }

        /// <summary>
        /// 层位
        /// </summary>
        [SugarColumn(ColumnDescription = "层位")]
        public string level { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [SugarColumn(ColumnDescription = "重量")]
        public double weight { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        [SugarColumn(ColumnDescription = "库区编码")]
        public string areaCode { get; set; }

        /// <summary>
        /// 来源库区
        /// </summary>
        [SugarColumn(ColumnDescription = "来源库区")]
        public string sourceCode { get; set; }

        /// <summary>
        /// 取货站台
        /// </summary>
        [SugarColumn(ColumnDescription = "取货站台")]
        public string s_station { get; set; }

        /// <summary>
        /// 放货站台
        /// </summary>
        [SugarColumn(ColumnDescription = "放货站台")]
        public string d_station { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        [SugarColumn(ColumnDescription = "货位")]
        public string location { get; set; }

        /// <summary>
        /// wmsID
        /// </summary>
        [SugarColumn(ColumnDescription = "wmsID")]
        public int wmsid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public int state { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        [SugarColumn(ColumnDescription = "priority")]
        public int priority { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime createTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [SugarColumn(ColumnDescription = "完成时间")]
        public DateTime endTime { get; set; }
    }
}