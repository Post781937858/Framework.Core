using Framework.Core.IServices.IBase;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.IServices
{
    public interface IMenuServices : IBaseServices<Menu>
    {
        Task<List<MenuView>> GetMenuViews();

        Task<List<MenuView>> GetMenuAllViews(Expression<Func<Menu, bool>> expression, Func<Menu, bool> expression1);

        Task<List<PermissionItemView>> PermissionItemViewsAsync(Expression<Func<PowerDetail, Menu, bool>> expression = null);
    }
}
