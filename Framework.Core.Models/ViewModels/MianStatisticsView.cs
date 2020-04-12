using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models.ViewModels
{
    public class MianStatisticsView
    {
        public int WarehouseA { get; set; }

        public int WarehouseCountA { get; set; }

        public int WarehouseB { get; set; }

        public int WarehouseCountB { get; set; }

        public int WarehouseF { get; set; }

        public int WarehouseCountF { get; set; }

        public string[] dataTime { get; set; }

        public List<int[]> Data { get; set; }
    }
}
