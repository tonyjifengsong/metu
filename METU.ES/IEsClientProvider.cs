using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES
{
    public interface IEsClientProvider
    {
        ElasticClient GetClient();
    }
}
