using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    public interface IStrategy
    {
        string StrategyName { get; set; }
        bool IsEnabledStrategy(IArgs context);
        object ApplyStategy(IArgs context);
        void DoWork(IArgs context);

    }
}
