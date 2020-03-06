using Framework.Core.Models;
using Framework.Core.Services.Base;
using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Core.Services
{
    public class PowerGroupServices : BaseServices<PowerGroup>, IPowerGroupServices
    {
        private readonly IPowerGroupRepository repository;

        public PowerGroupServices(IPowerGroupRepository Repository) : base(Repository)
        {
            repository = Repository;
        }

        public Task<PageModel<PowerGroupView>> GetPowerGroupViews(Expression<Func<PowerGroup, User, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
        {
            return repository.GetPowerGroupViews(whereExpression, intPageIndex, intPageSize, strOrderByFileds);
        }
    }
}
