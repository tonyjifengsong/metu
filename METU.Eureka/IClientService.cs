using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.Eureka
{
    public interface IClientService
    {
        Task<string> GetClientName(int clientId);
    }
}
