using Framework.Core.IRepository.IUnitOfWork.IBase;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.IRepository
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<List<MenuView>> GetMenuViews();

        Task<List<MenuView>> GetMenuAllViews(Expression<Func<Menu, bool>> expression);
    }

}
