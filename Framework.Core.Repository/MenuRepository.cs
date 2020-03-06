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
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuView>> GetMenuAllViews(Expression<Func<Menu, bool>> expression)
        {
            List<MenuView> menuViews = new List<MenuView>();
            if (!string.IsNullOrEmpty(_user.Role))
            {
                var ListMenu = await base.Query(expression);
                ListMenu.Where(p => (p.menutype == menuType.Menu && p.menuid == 999) || p.menutype == menuType.Button || p.menutype == menuType.Api)
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
        /// 查询当前用户所拥有权限菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuView>> GetMenuViews()
        {
            List<MenuView> menuViews = new List<MenuView>();
            if (!string.IsNullOrEmpty(_user.Role))
            {
                var ListMenu = await base.Db.Queryable<PowerDetail, Menu>((r, b) => new object[] { JoinType.Right, r.menuid == b.Id }).Where((r, b) => r.PowerName == _user.Role).Select((r, b) => b).ToListAsync();
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
        /// 递归查询所有子菜单
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
    }
}
