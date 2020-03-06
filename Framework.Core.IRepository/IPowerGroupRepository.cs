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
    public interface IPowerGroupRepository : IBaseRepository<PowerGroup>
    {
        Task<PageModel<PowerGroupView>> GetPowerGroupViews(Expression<Func<PowerGroup, User, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);
    }

}
