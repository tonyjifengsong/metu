using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    public interface IModelBase : IBase
    {
       string Name { get; set; }
    }
}
