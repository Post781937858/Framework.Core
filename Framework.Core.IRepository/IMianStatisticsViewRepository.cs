using Framework.Core.IRepository.IUnitOfWork.IBase;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.IRepository
{
    public interface IMianStatisticsViewRepository : IBaseRepository<MianStatisticsView>
    {
        Task<List<InforesultView>> GetStatisticsViewAsync(string area_code,int DataCount);


        Task<InforesultView> GetTodayOrderCountAsync(string area_code);

        Task<InforesultView> GetOrderCountAsync(string area_code);
    }
}
