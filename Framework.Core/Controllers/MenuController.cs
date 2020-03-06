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

        public MenuController(IMenuServices menuServices,IMapper mapper)
        {
            this._menuServices = menuServices;
            this.mapper = mapper;
        }

        /// <summary>
        /// 查询当前用户菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<MenuView>>> GetMenuAsync()
        {
            return new MessageModel<List<MenuView>>(await _menuServices.GetMenuViews());
        }

        /// <summary>
        /// 获取所有菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<MessageModel<List<MenuView>>> GetMenuALLAsync([FromQuery]MenuView menuView)
        {
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
            return new MessageModel<List<MenuView>>(await _menuServices.GetMenuAllViews(whereExpression));
        }



        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="powerGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(MenuView powerGroup)
        {
            powerGroup.Id = 0;
            var model = mapper.Map<Menu>(powerGroup);
            return new MessageModel(await _menuServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="powerGroup"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(MenuView powerGroup)
        {
            return new MessageModel(await _menuServices.Update(powerGroup));
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="powerGroupList"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<MenuView> powerGroupList)
        {
            List<object> Ids = new List<object>();
            GetIds(ref Ids, powerGroupList);
            return new MessageModel(await _menuServices.DeleteByIds(Ids.ToArray()));
        }

        /// <summary>
        /// 递归获取所有ID
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="powerGroupList"></param>
        private void GetIds(ref List<object> Ids, List<MenuView> powerGroupList)
        {
            foreach (var item in powerGroupList)
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