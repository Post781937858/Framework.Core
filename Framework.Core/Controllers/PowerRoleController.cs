using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Core.Common;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IPowerDetailServices powerDetailServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PowerRoleController(IPowerGroupServices powerGroupServices, IUser user, IPowerDetailServices powerDetailServices, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._powerGroupServices = powerGroupServices;
            this.user = user;
            this.powerDetailServices = powerDetailServices;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
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

        [HttpPost("SaveRolePower")]
        public async Task<MessageModel> SaveRolePower(PowerRoleView powerRole)
        {
            try
            {
                unitOfWork.BeginTran();
                object[] Ids = new object[powerRole.UserPower.Length];
                powerRole.UserPower.CopyTo(Ids, 0);
                var result = await powerDetailServices.Delete(p => p.PowerName == powerRole.powerGroup.name);
                List<PowerDetail> menuViews = new List<PowerDetail>();
                powerRole.MenuViewList.ForEach(item =>
                {
                    menuViews.Add(new PowerDetail() { menuid = item.Id, PowerName = powerRole.powerGroup.name });
                    if (item.submenuApi != null)
                    {
                        item.submenuApi.ForEach(p =>
                        {
                            if (p.Checked)
                            {
                                menuViews.Add(new PowerDetail() { menuid = p.Id, PowerName = powerRole.powerGroup.name });
                            }
                        });
                    }
                });
                await powerDetailServices.Add(menuViews);
                unitOfWork.CommitTran();
            }
            catch (Exception)
            {
                unitOfWork.RollbackTran();
            }
            return new MessageModel();
        }
    }

    public class PowerRoleView
    {
        public List<MenuView> MenuViewList { get; set; }

        public  PowerGroup powerGroup { get; set; }


        public int[] UserPower { get; set; }
    }
}