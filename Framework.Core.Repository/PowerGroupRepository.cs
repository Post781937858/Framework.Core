using AutoMapper;
using Framework.Core.IRepository;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Framework.Core.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Repository
{
    public class PowerGroupRepository : BaseRepository<PowerGroup>, IPowerGroupRepository
    {

        public PowerGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageModel<PowerGroupView>> GetPowerGroupViews(Expression<Func<PowerGroup, User, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
        {
            RefAsync<int> totalCount = 0;
            var data = await base.Db.Queryable<PowerGroup, User>((r, b) => new object[] { JoinType.Left, r.CreateUserId == b.Id })
             .WhereIF(whereExpression != null, whereExpression)
             .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
             .Select((r, b) => new PowerGroupView { Id = r.Id, name = r.name, explain = r.explain, showName = b.showName, CreateTime = r.CreateTime , CreateUserId =r.CreateUserId})
             .ToPageListAsync(intPageIndex, intPageSize, totalCount);

            int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
            return new PageModel<PowerGroupView>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = data };
        }
    }
}
