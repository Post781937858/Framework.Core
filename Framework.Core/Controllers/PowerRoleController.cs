using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    public class PowerRoleController : ControllerBase
    {
        private readonly IPowerGroupServices _powerGroupServices;
        private readonly IUser user;

        public PowerRoleController(IPowerGroupServices powerGroupServices, IUser user)
        {
            this._powerGroupServices = powerGroupServices;
            this.user = user;
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<PowerGroupView>>> Query(int Pageindex, int PageSize = 10, string Role = "", string user = "")
        {
            Expression<Func<PowerGroup, User, bool>> whereExpression = null;
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(Role))
            {
                whereExpression = (r, b) => r.name.Contains(Role) || b.showName.Contains(user);
            }
            else if (!string.IsNullOrEmpty(Role))
            {
                whereExpression = (r, b) => r.name.Contains(Role);
            }
            else if (!string.IsNullOrEmpty(user))
            {
                whereExpression = (r, b) => b.showName.Contains(user);
            }
            var data = await _powerGroupServices.GetPowerGroupViews(whereExpression, Pageindex, PageSize);
            return new MessageModel<PageModel<PowerGroupView>>(data);
        }

        /// <summary>
        /// 查询全部角色数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<MessageModel<List<PowerGroup>>> Query()
        {
            var data = await _powerGroupServices.Query();
            return new MessageModel<List<PowerGroup>>(data);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="powerGroup"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(PowerGroup powerGroup)
        {
            powerGroup.Id = 0;
            powerGroup.CreateUserId = user.ID;
            powerGroup.CreateTime = DateTime.Now;
            return new MessageModel(await _powerGroupServices.Add(powerGroup) > 0);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="powerGroup"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(PowerGroup powerGroup)
        {
            return new MessageModel(await _powerGroupServices.Update(powerGroup));
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="powerGroupList"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<PowerGroup> powerGroupList)
        {
            List<object> Ids = new List<object>();
            powerGroupList.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await _powerGroupServices.DeleteByIds(Ids.ToArray()));
        }
    }
}