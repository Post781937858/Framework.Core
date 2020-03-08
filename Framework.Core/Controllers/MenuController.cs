using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Framework.Core.Common;
using AutoMapper;

namespace Framework.Core.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuServices _menuServices;
        private readonly IMapper mapper;

        public MenuController(IMenuServices menuServices, IMapper mapper)
        {
            this._menuServices = menuServices;
            this.mapper = mapper;
        }

        /// <summary>
        /// 查询当前用户菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<MenuView>>> GetMenuAsync()
        {
            return new MessageModel<List<MenuView>>(await _menuServices.GetMenuViews());
        }

        /// <summary>
        /// 查询所有菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<MessageModel<List<MenuView>>> GetMenuALLAsync([FromQuery]MenuView menuView)
        {
            Func<Menu, bool> whereExpressionAll = p => (p.menutype == menuType.Menu && p.menuid == 999) || p.menutype == menuType.Button || p.menutype == menuType.Api;
            Expression<Func<Menu, bool>> whereExpression = r => true;
            Expression<Func<Menu, bool>> whereExpression1 = r => true;
            Expression<Func<Menu, bool>> whereExpression2 = r => true;
            Expression<Func<Menu, bool>> whereExpression3 = r => true;
            if (!string.IsNullOrEmpty(menuView.title))
            {
                whereExpression1 = r => r.title.Contains(menuView.title);
                whereExpression = whereExpression.And(whereExpression1);
            }
            if (menuView.state != 0)
            {
                whereExpression2 = r => r.state == menuView.state;
                whereExpression = whereExpression.And(whereExpression2);
            }
            if (menuView.menutype != menuType.ALL)
            {
                whereExpression3 = r => r.menutype == menuView.menutype;
                whereExpression = whereExpression.And(whereExpression3);
            }
            return new MessageModel<List<MenuView>>(await _menuServices.GetMenuAllViews(whereExpression, whereExpressionAll));
        }

        /// <summary>
        /// 查询菜单 接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("Power")]
        public async Task<MessageModel<List<MenuView>>> GetPowerAsync()
        {
            Func<Menu, bool> whereExpressionAll = p => (p.menutype == menuType.Menu && p.menuid == 999);
            return new MessageModel<List<MenuView>>(await _menuServices.GetMenuAllViews(null, whereExpressionAll));
        }

        [HttpGet("UserPower")]
        public async Task<MessageModel<int[]>> UserPower(string role)
        {
            var data = await _menuServices.PermissionItemViewsAsync((r, b) => r.PowerName == role);
            return new MessageModel<int[]>(data.Select(p => p.id).ToArray());
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="MenuView"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(MenuView MenuView)
        {
            MenuView.Id = 0;
            if (MenuView.menutype == menuType.Menu)
            {
                if (MenuView.menuid == 0)
                {
                    MenuView.menuid = 999;
                }
            }
            var model = mapper.Map<Menu>(MenuView);
            return new MessageModel(await _menuServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="MenuView"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(MenuView MenuView)
        {
            if (MenuView.menutype == menuType.Menu)
            {
                if (MenuView.menuid == 0)
                {
                    MenuView.menuid = 999;
                }
            }
            return new MessageModel(await _menuServices.Update(MenuView));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="MenuViewList"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<MenuView> MenuViewList)
        {
            List<object> Ids = new List<object>();
            GetIds(ref Ids, MenuViewList);
            return new MessageModel(await _menuServices.DeleteByIds(Ids.ToArray()));
        }

        /// <summary>
        /// 递归获取所有ID
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="MenuViewList"></param>
        private void GetIds(ref List<object> Ids, List<MenuView> MenuViewList)
        {
            foreach (var item in MenuViewList)
            {
                Ids.Add(item.Id);
                if (item.submenu != null && item.submenu.Any())
                {
                    GetIds(ref Ids, item.submenu);
                }
            }
        }
    }
}