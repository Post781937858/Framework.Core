using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models.ViewModels
{
    public class PermissionItemView : IMapperTo<Menu>
    {

        public virtual int id { get; set; }

        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Role { get; set; }


        public virtual string method { get; set; }

        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}
