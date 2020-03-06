using Framework.Core.IRepository;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.Models;
using Framework.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
