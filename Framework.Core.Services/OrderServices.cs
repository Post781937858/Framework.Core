using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Services
{
    public class OrderService : BaseServices<Order>, IOrderServices
    {
        public OrderService(IOrderRepository Repository) : base(Repository)
        {

        }
    }
}
