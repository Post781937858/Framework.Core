using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Services
{
    public class line_checkService : BaseServices<line_check>, Iline_checkServices
    {
        public line_checkService(Iline_checkRepository Repository) : base(Repository)
        {

        }
    }
}
