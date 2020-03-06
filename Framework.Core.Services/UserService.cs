using Framework.Core.IRepository;
using Framework.Core.IServices;
using Framework.Core.Models;
using Framework.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Services
{
    public class UserService : BaseServices<User>, IUserServices
    {
        public UserService(IUserRepository Repository) : base(Repository)
        {

        }
    }
}
