using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models.ViewModels
{
    public class MenuView : IMapperTo<Menu>
    {
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// icon图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// url地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int state { get; set; }


        /// <summary>
        /// 菜单类型
        /// </summary>
        public menuType menutype { get; set; }

        /// <summary>
        /// 子菜单ID
        /// </summary>
        public int menuid { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string explain { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int no { get; set; }


        public List<MenuView> submenu { get; set; }
       
    }
}
