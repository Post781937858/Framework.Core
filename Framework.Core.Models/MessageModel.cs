using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel
    {

        public MessageModel(bool _state)
        {
            this.state = _state ? 200 : 500;
            this.msg = _state ? "成功" : "失败";
        }


        /// <summary>
        /// 操作是否成功
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }

    }

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel<T> : MessageModel
    {

        public MessageModel(T data,bool _state = true) : base(_state)
        {
            this.data = data;
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }
    }
}
