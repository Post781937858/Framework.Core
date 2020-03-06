using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Framework.Core.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.IServices;
using Framework.Core.IRepository;
using System.Linq.Expressions;
using System;

namespace Framework.Core.Services
{
    public class MenuServices : BaseServices<Menu>, IMenuServices
    {
        private readonly IMenuRepository menuRepository;

        public MenuServices(IMenuRepository menuRepository) : base(menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        public Task<List<MenuView>> GetMenuAllViews(Expression<Func<Menu, bool>> expression)
        {
            return menuRepository.GetMenuAllViews(expression);
        }

        public  Task<List<MenuView>> GetMenuViews()
        {
            return  menuRepository.GetMenuViews();
        }

    }
}
