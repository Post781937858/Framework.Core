using Framework.Core.IRepository;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Framework.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Repository
{
    public class MianStatisticsViewRepository : BaseRepository<MianStatisticsView>, IMianStatisticsViewRepository
    {
        public MianStatisticsViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<InforesultView>> GetStatisticsViewAsync(string area_code,int DataCount)
        {
            var result = await base.Db.SqlQueryable<InforesultView>($"select DATE_FORMAT(create_time,'%Y-%m-%d') as time,Count(*) as count  from `line_check`   where  area_code='{area_code}' AND messageName!='packetPass' AND DATE_SUB(CURDATE(), INTERVAL {DataCount} DAY) <= date(create_time) GROUP BY time").ToListAsync();
            return result;
        }

        public async Task<InforesultView> GetTodayOrderCountAsync(string area_code)
        {
            var result = await base.Db.SqlQueryable<InforesultView>($"select DATE_FORMAT(create_time,'%Y-%m-%d') as time,count(*) count  from `line_check`   where  area_code='{area_code}' AND messageName!='packetPass' AND DATE_FORMAT(create_time,'%Y-%m-%d')='{DateTime.Now.ToString("yyyy-MM-dd")}' GROUP BY time").FirstAsync();
            return result;
        }

        public async Task<InforesultView> GetOrderCountAsync(string area_code)
        {
            var result = await base.Db.SqlQueryable<InforesultView>($"select Count(id) as count From `line_check` where  area_code='{area_code}'").FirstAsync();
            return result;
        }
    }
}
