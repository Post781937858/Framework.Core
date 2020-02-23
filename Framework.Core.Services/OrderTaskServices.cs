using Framework.Core.Common;
using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Services
{
    public class OrderTaskServices : BaseServices<OrderTask>, IOrderTaskServices
    {
        readonly IOrderTaskRepository _orderTaskRepository;
        public OrderTaskServices(IOrderTaskRepository taskRepository) : base(taskRepository)
        {
            _orderTaskRepository = taskRepository;
        }

        [Caching(AbsoluteExpiration = 10)]
        public Task<List<OrderTask>> GetOrder()
        {
            return Task.Run(() =>
            {
                var listOrder = new List<OrderTask>();
                listOrder.Add(new OrderTask());
                listOrder.Add(new OrderTask());
                listOrder.Add(new OrderTask());
                return listOrder;
            });
        }

        [Caching(AbsoluteExpiration = 10)]
        public void TaskOrder()
        {
        }
    }
}
