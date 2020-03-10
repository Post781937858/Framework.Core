using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Services
{
    public class operatingLogServices : BaseServices<operatingLog>, IoperatingLogServices
    {

        public operatingLogServices(IoperatingLogRepository Repository) : base(Repository)
        {

        }
    }
}
