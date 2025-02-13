using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace METU.RabbitMQ
{
    public class RabbitListener : IHostedService
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        public RabbitListener(IOptions<RabbitConfig> options)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    // 这是我这边的配置,自己改成自己用就好
                    HostName = options.Value.RabbitHost,
                    UserName = options.Value.RabbitUserName,
                    Password = options.Value.RabbitPassword,
                    Port = options.Value.RabbitPort,
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }
        protected string RouteKey;
        protected string QueueName;
        // 处理消息的方法
        public virtual bool Process(string message)
        {
            throw new NotImplementedException();
        }
        // 注册消费者监听在这里
        public void Register()
        {
            Console.WriteLine($"RabbitListener register,routeKey:{RouteKey}");
            channel.ExchangeDeclare(exchange: "message", type: "topic");
            channel.QueueDeclare(queue: QueueName, exclusive: false);
            channel.QueueBind(queue: QueueName,
                              exchange: "message",
                              routingKey: RouteKey);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message =  body.ToString();
                var result = Process(message);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: QueueName, consumer: consumer);
        }
        public void DeRegister()
        {
            this.connection.Close();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.connection.Close();
            return Task.CompletedTask;
        }
    }
}
