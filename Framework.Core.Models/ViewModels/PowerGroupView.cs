using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models.ViewModels
{
    public class PowerGroupView : IMapperTo<PowerGroup>
    {

        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string explain { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string showName { get; set; }

        public int CreateUserId { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
