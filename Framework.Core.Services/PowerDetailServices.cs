using Framework.Core.Models;
using Framework.Core.Services.Base;
using Framework.Core.IRepository;
using Framework.Core.IServices;

namespace Framework.Core.Services
{
    public class PowerDetailServices : BaseServices<PowerDetail>, IPowerDetailServices
    {

        public PowerDetailServices(IPowerDetailRepository Repository) : base(Repository)
        {
        }
    }
}
