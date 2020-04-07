using AutoMapper;
using Framework.Core.Common;
using Framework.Core.IRepository;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Framework.Core.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Repository
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        private readonly IMapper mapper;
        private readonly IPowerDetailRepository powerDetailRepository;
        private readonly IUser _user;

        public MenuRepository(IUnitOfWork unitOfWork, IMapper mapper, IPowerDetailRepository powerDetailRepository, IUser user) : base(unitOfWork)
        {
            this.mapper = mapper;
            this.powerDetailRepository = powerDetailRepository;
            this._user = user;
        }

        /// <summary>
        /// 查询所有角色所拥有权限
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<PermissionItemView>> PermissionItemViewsAsync(Expression<Func<Menu, PowerDetail, bool>> expression = null)
        {
            return await base.Db.Queryable<Menu, PowerDetail>((b, r) => new object[] { JoinType.Left, r.menuid == b.Id })
                .WhereIF(expression != null, expression)
                .Select((b, r) => new PermissionItemView() { Url = b.url, Role = r.PowerName, method = b.method, id = b.Id }).ToListAsync();
        }

        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuView>> GetMenuAllViews(Expression<Func<Menu, bool>> expression, Func<Menu, bool> expression1)
        {
            List<MenuView> menuViews = new List<MenuView>();
            var ListMenu = await base.Query(expression);
            ListMenu.Where(expression1)
            .ToList().ForEach(p =>
            {
                MenuView item = mapper.Map<MenuView>(p);
                menuViews.Add(item);
            });
            GetSubmenuMenuApiView(ref menuViews, ListMenu);
            return menuViews;
        }

        /// <summary>
        /// 查询当前用户所拥有权限菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuView>> GetMenuViews()
        {
            List<MenuView> menuViews = new List<MenuView>();
            if (!string.IsNullOrEmpty(_user.Role))
            {
                var ListMenu = await base.Db.Queryable<Menu, PowerDetail>((b,r) => new object[] { JoinType.Left, r.menuid == b.Id }).Where((b, r) => r.PowerName == _user.Role && b.menutype == menuType.Menu).Select((b, r) => b).ToListAsync();
                ListMenu.Where(p => p.menuid == 999 && p.menutype == menuType.Menu)
                .ToList().ForEach(p =>
                {
                    MenuView item = mapper.Map<MenuView>(p);
                    menuViews.Add(item);
                });
                GetSubmenuMenuView(ref menuViews, ListMenu);
            }
            return menuViews;
        }

        /// <summary>
        /// 递归查询所有子菜单无菜单接口
        /// </summary>
        /// <param name="menuViews"></param>
        /// <param name="menus"></param>
        private void GetSubmenuMenuView(ref List<MenuView> menuViews, IEnumerable<Menu> menus)
        {
            menuViews.ForEach(p =>
            {
                List<MenuView> ViewsSubmenu = new List<MenuView>();
                var ListMenus = menus.Where(s => s.menuid == p.Id);
                if (ListMenus.Any())
                {
                    ListMenus.ToList().ForEach(i =>
                    {
                        MenuView item = mapper.Map<MenuView>(i);
                        ViewsSubmenu.Add(item);
                    });
                    p.submenu = ViewsSubmenu;
                    GetSubmenuMenuView(ref ViewsSubmenu, menus);
                }
            });
        }

        /// <summary>
        /// 递归查询所有子菜单含菜单接口
        /// </summary>
        /// <param name="menuViews"></param>
        /// <param name="menus"></param>
        private void GetSubmenuMenuApiView(ref List<MenuView> menuViews, IEnumerable<Menu> menus)
        {
            menuViews.ForEach(p =>
            {
                List<MenuView> ViewsSubmenu = new List<MenuView>();
                var ListMenus = menus.Where(s => s.menuid == p.Id && s.menutype == menuType.Menu);
                if (ListMenus.Any())
                {
                    ListMenus.ToList().ForEach(i =>
                    {
                        MenuView item = mapper.Map<MenuView>(i);
                        ViewsSubmenu.Add(item);
                    });
                    p.submenu = ViewsSubmenu;
                    GetSubmenuMenuApiView(ref ViewsSubmenu, menus);
                }
                List<MenuView> ViewsSubmenuApi = new List<MenuView>();
                var ListMenusAPI = menus.Where(s => s.menuid == p.Id && s.menutype != menuType.Menu);
                if (ListMenusAPI.Any())
                {
                    ListMenusAPI.ToList().ForEach(i =>
                    {
                        MenuView item = mapper.Map<MenuView>(i);
                        ViewsSubmenuApi.Add(item);
                    });
                    p.submenuApi = ViewsSubmenuApi;
                    GetSubmenuMenuApiView(ref ViewsSubmenuApi, menus);
                }
            });
        }
    }
}
