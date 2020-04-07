using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    [ModelDescription(Description = "任务模型")]
    public class line_check : RootEntity
    {

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "任务ID")]
        public int line_id { get; set; }

        /// <summary>
        /// 料箱号
        /// </summary>
        [SugarColumn(ColumnDescription = "料箱号")]
        public string box_id { get; set; }

        /// <summary>
        /// wmsid
        /// </summary>
        [SugarColumn(ColumnDescription = "WMSID")]
        public string wms_id { get; set; }

        /// <summary>
        /// 目的地“A12,A13,A14,A20"
        /// </summary>
        [SugarColumn(ColumnDescription = "库区编码")]
        public string area_code { get; set; }

        /// <summary>
        /// 源地址
        /// </summary>
        [SugarColumn(ColumnDescription = "源地址")]
        public string source_code { get; set; }


        /// <summary>
        /// 1.1.1m托盘 2.1.4m托盘
        /// </summary>
        [SugarColumn(ColumnDescription = "托盘大小")]
        public int size { get; set; }

        /// <summary>
        /// 状态 0未发送  3已完成未反馈  4 完成
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public tasksstate state { get; set; }

        /// <summary>
        /// 1.料盒  2.托盘
        /// </summary>
        [SugarColumn(ColumnDescription = "类型")]
        public int type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "消息类型")]
        public string messageName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "位置")]
        public string location { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "新库位")]
        public string d_location { get; set; }

        /// <summary>
        /// 取货站台
        /// </summary>
        [SugarColumn(ColumnDescription = "原库位")]
        public string s_location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "优先级")]
        public int priority { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "重量")]
        public int weight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "Json")]
        public string json { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "取货站台")]
        public int s_station { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "放货站台")]
        public int d_station { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime create_time { get; set; }

        /// <summary>
        /// 是否已发给过终端，0未发送 1.已发送
        /// </summary>
        [SugarColumn(ColumnDescription = "发送状态")]
        public sendstate sendSwitch { get; set; }

        /// <summary>
        /// 是否已将重量发给wms0.未发送  1.已发送未回复 2.已发送已回复
        /// </summary>
        [SugarColumn(ColumnDescription = "重量发送状态")]
        public weightSend weightSendSwitch { get; set; }

    }

    public enum tasksstate
    {
        all = 99,
        initial = 0,
        awaits = 3,
        achieve = 4
    }

    public enum sendstate
    {
        all = 99,
        awaits = 0,
        achieve = 1
    }

    public enum weightSend
    {
        all = 99,
        awaits = 0,
        achieve = 1
    }
}
