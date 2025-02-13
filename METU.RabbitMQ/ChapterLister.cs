using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Realms.Sync;

namespace METU.RabbitMQ
{
    public class ChapterLister : RabbitListener
    {
        private readonly ILogger<RabbitListener> _logger;
        // 因为Process函数是委托回调,直接将其他Service注入的话两者不在一个scope,
        // 这里要调用其他的Service实例只能用IServiceProvider CreateScope后获取实例对象
        private readonly IServiceProvider _services;
        public ChapterLister(IServiceProvider services, IOptions<RabbitConfig> options,
         ILogger<RabbitListener> logger) : base(options)
        {
            base.RouteKey = "done.task";
            base.QueueName = "lemonnovelapi.chapter";
            _logger = logger;
            _services = services;
        }
        public override bool Process(string message)
        {
            var taskMessage = JToken.Parse(message);
            if (taskMessage == null)
            {
                // 返回false 的时候回直接驳回此消息,表示处理不了
                return false;
            }
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var xxxService = scope.ServiceProvider.GetRequiredService<ChapterLister>();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Process fail,error:{ex.Message},stackTrace:{ex.StackTrace},message:{message}");
                _logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}
