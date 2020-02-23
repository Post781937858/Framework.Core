using Framework.Core.IServices.IBase;
using Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.IServices
{
    public interface IOrderTaskServices: IBaseServices<OrderTask>
    {
        Task<List<OrderTask>> GetOrder();

        void TaskOrder();
    }
}
