using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    public class Userinfo
    {
        public int id { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int Power_ID { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int UserState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string showName { get; set; }

        /// <summary>
        ///图像 
        /// </summary>
        public string Imgurl { get; set; }


        /// <summary>
        /// 原密码
        /// </summary>
        public string originalPwd { get; set; }
    }
}
