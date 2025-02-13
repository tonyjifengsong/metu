using METU.Quartz;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace METU.API.Controllers
{/// <summary>
/// 
/// </summary>
    public class MyJob : ITonyJob//创建IJob的实现类，并实现Excute方法。
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var types = ASMHelper.GetClass("dowork");
                var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith("dowork", StringComparison.OrdinalIgnoreCase));
                foreach (var interfaceType in interfaces)
                {
                    var implementType = types.FirstOrDefault(m => m != interfaceType && !m.IsInterface && interfaceType.IsAssignableFrom(m));
                    if (implementType != null)
                    {
                        try
                        {
                            IDoWork fb = (IDoWork)Activator.CreateInstance(implementType, null);
                            fb.dowork();
                        }catch(Exception ex)
                        {
                            FileHelpers.Writelog(implementType.GetType().FullName);
                            FileHelpers.Writelog(ex.Message);
                        }
                       
                    }
                }
            });
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTimerConfig()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetTimerFrequent()
        {
            return 2;
        }
    }
}
