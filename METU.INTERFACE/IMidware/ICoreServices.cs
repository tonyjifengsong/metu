using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public interface ICoreServices
    {
        void AddService(IServiceCollection services, string endwith = "BLL");
    }
}
