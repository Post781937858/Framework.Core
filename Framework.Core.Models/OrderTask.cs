using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    public class OrderTask
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public int mesid { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string messageName { get; set; }

        /// <summary>
        /// 箱号ID
        /// </summary>
        public int boxId { get; set; }

        /// <summary>
        /// 层位
        /// </summary>
        public string level { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// 库区编码
        /// </summary>
        public string areaCode { get; set; }

        /// <summary>
        /// 来源库区
        /// </summary>
        public string sourceCode { get; set; }

        /// <summary>
        /// 取货站台
        /// </summary>
        public string s_station { get; set; }

        /// <summary>
        /// 放货站台
        /// </summary>
        public string d_station { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// wmsID
        /// </summary>
        public int wmsid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority { get; set; }
    }
}
