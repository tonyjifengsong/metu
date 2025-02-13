using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    /// <summary>
    /// Created by tony
    /// </summary>
    public interface IServiceFilter
    {
        bool DoFilter(Dictionary<string, object> dic = null);

    }
}
