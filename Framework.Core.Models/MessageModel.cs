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

        public MessageModel(bool _state=true, string msg = "")
        {
            this.state = _state ? 200 : 500;
            if (string.IsNullOrEmpty(msg))
            {
                this.msg = _state ? "操作成功" : "操作失败";
            }
            else
            {
                this.msg = msg;
            }
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

        public MessageModel(T data, bool _state = true, string msg = "") : base(_state, msg)
        {
            this.data = data;
        }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }
    }
}
