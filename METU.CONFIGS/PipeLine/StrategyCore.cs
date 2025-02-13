using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.CONFIGS.PipeLine
{
    public class StrategyCore : IStrategy
    {
        public string StrategyName { get; set; }

        public  virtual object ApplyStategy(IArgs context)
        {
            if (StrategyName == null) StrategyName = DateTime.Now.ToString();
            FileHelper.Writelog("Strategy["+ StrategyName + "] is execute");
            return new object();
        }

        public virtual void DoWork(IArgs context)
        {
            if (context == null)
            {
                FileHelper.Writelog("DoWork param context is null[" + StrategyName + "] is execute");
            }
            else
            {
                FileHelper.Writelog("DoWork param context is :" + context.toJson());
            }
        }

        public virtual bool IsEnabledStrategy(IArgs context)
        {
            if (context == null)
            {
                FileHelper.Writelog("param context is null[" + StrategyName + "] is execute");

            }
            else
            {
                FileHelper.Writelog("param context is :" + context.toJson());
            }

            return false;
        }
    }
}
