using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Services
{
    public class MianStatisticsViewService : BaseServices<MianStatisticsView>, IMianStatisticsViewServices
    {
        private readonly IMianStatisticsViewRepository repository;

        public MianStatisticsViewService(IMianStatisticsViewRepository Repository) : base(Repository)
        {
            repository = Repository;
        }

        public async Task<MianStatisticsView> GetMainStatisticsViewAsync()
        {
            List<int[]> DataResult = new List<int[]>();
            List<string> Time = new List<string>();
            for (int i = 0; i < 30; i++)
            {
                Time.Add(DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"));
            }
            Time.Reverse();
            MianStatisticsView statisticsView = new MianStatisticsView();
            statisticsView.dataTime = Time.ToArray();
            var resultA = await repository.GetStatisticsViewAsync("A13", Time.Count);
            var resultB = await repository.GetStatisticsViewAsync("A12", Time.Count);
            var resultF = await repository.GetStatisticsViewAsync("A14", Time.Count);
            DataResult.Add(GetData(Time, resultA));
            DataResult.Add(GetData(Time, resultB));
            DataResult.Add(GetData(Time, resultF));
            statisticsView.Data = DataResult;
            return statisticsView;
        }

        public async Task<MianStatisticsView> GetTagViewAsync()
        {
            MianStatisticsView statisticsView = new MianStatisticsView();
            var resultTodayA = await repository.GetTodayOrderCountAsync("A13");
            var resultTodayB = await repository.GetTodayOrderCountAsync("A12");
            var resultTodayF = await repository.GetTodayOrderCountAsync("A14");
            var resultA = await repository.GetOrderCountAsync("A13");
            var resultB = await repository.GetOrderCountAsync("A12");
            var resultF = await repository.GetOrderCountAsync("A14");
            statisticsView.WarehouseA = resultTodayA != null ? resultTodayA.count : 0;
            statisticsView.WarehouseB = resultTodayB != null ? resultTodayB.count : 0;
            statisticsView.WarehouseF = resultTodayF != null ? resultTodayF.count : 0;
            statisticsView.WarehouseCountA = resultA != null ? resultA.count : 0;
            statisticsView.WarehouseCountB = resultB != null ? resultB.count : 0;
            statisticsView.WarehouseCountF = resultF != null ? resultF.count : 0;
            return statisticsView;
        }

        private int[] GetData(List<string> Time, List<InforesultView> views)
        {
            List<int> data = new List<int>();
            Time.ForEach(p =>
            {
                var res = views.FirstOrDefault(s => s.time == p);
                if (res != null)
                {
                    data.Add(res.count);
                }
                else
                {
                    data.Add(0);
                }
            });
            return data.ToArray();
        }
    }
}
