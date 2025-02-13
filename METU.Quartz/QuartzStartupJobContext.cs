using METU.Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;
using IOCContainer = System.IServiceProvider;
// Quartz.Net启动后注册job和trigger

namespace Quartz
{
    public class QuartzStartupJobContext: IJobContext
    {
        public IScheduler _scheduler { get; set; }

        private readonly IJobFactory iocJobfactory;
        public QuartzStartupJobContext(IOCContainer IocContainer)
        {

            iocJobfactory = new IOCJobFactory(IocContainer);
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = iocJobfactory;
        }
        public void Start()
        {
            var types = ASMHelper.GetMidwareAllClass();
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.Name.ToString().ToLower() != "ijob".ToString().ToLower() && t.FullName.EndsWith("Job", StringComparison.OrdinalIgnoreCase));
            foreach (var interfaceType in interfaces)
            {
                var implementType = types.FirstOrDefault(m => m != interfaceType && !m.IsInterface && m.FullName.EndsWith("Job", StringComparison.OrdinalIgnoreCase) && interfaceType.IsAssignableFrom(m));

                if (implementType != null)
                {
                    ITonyJob fb = (ITonyJob)Activator.CreateInstance(implementType, null);
                    // string timeconfig = fb.GetTimerConfig();
                    int seconds = 2;
                    try
                    {
                        seconds = fb.GetTimerFrequent();
                    }
                    catch { seconds = 0; }
                    if (seconds < 1) seconds = 2;
                   // if (timeconfig == null) timeconfig = "1 * * ** *";
                    // System.Diagnostics.Debug.WriteLine(fb.GetTimerConfig());

                    string IdentityName = "tony" + implementType.Name;


                    _scheduler.Start().Wait();
                    var UsageCounterSyncJob = JobBuilder.Create(implementType)
                       .WithIdentity(IdentityName)
                       .Build();
                    var UsageCounterSyncJobTrigger = TriggerBuilder.Create()
                        .WithIdentity(IdentityName)
                        .StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds).RepeatForever())//每两秒执行一次

                        // 每隔一小时同步一次
                        // .WithCronSchedule(timeconfig)      // Seconds,Minutes,Hours，Day-of-Month，Month，Day-of-Week，Year（optional field）
                        .Build();
                    try
                    {
                        _scheduler.ScheduleJob(UsageCounterSyncJob, UsageCounterSyncJobTrigger).Wait();

                        _scheduler.TriggerJob(new JobKey(IdentityName));
                    }
                    catch (Exception ex)
                    {
                      //  FileHelpers.Writelog(ex.Message);
                    }
                }
            }

        }

        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                _scheduler = null;
            else
            {
            }

        }
    }
    /// <summary>
    /// IOCJobFactory ：实现在Timer触发的时候注入生成对应的Job组件
    /// </summary>
    public class IOCJobFactory : IJobFactory
    {
        protected readonly IOCContainer Container;
        public IOCJobFactory(IOCContainer container)
        {
            Container = container;
        }
        //Called by the scheduler at the time of the trigger firing, in order to produce
        //a Quartz.IJob instance on which to call Execute.
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return Container.GetService(bundle.JobDetail.JobType) as IJob;
        }
        // Allows the job factory to destroy/cleanup the job if needed.
        public void ReturnJob(IJob job)
        {
        }
    }


}
