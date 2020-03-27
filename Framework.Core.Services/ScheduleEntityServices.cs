using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Services
{
    public class ScheduleEntityService : BaseServices<ScheduleEntity>, IScheduleEntityServices
    {
        public ScheduleEntityService(IScheduleEntityRepository Repository) : base(Repository)
        {

        }
    }
}
