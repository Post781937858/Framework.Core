using Framework.Core.IRepository;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.Models;
using Framework.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Repository
{
    public class OrderTaskRepository : BaseRepository<OrderTask>, IOrderTaskRepository
    {
        public OrderTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
