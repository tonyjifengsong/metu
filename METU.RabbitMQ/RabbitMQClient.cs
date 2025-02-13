using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace METU.RabbitMQ
{
    public class RabbitMQClient
    {
        private readonly IModel _channel;
        private readonly ILogger _logger;
        public RabbitMQClient(IOptions<RabbitConfig> options, ILogger<RabbitMQClient> logger)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = options.Value.RabbitHost,
                    UserName = options.Value.RabbitUserName,
                    Password = options.Value.RabbitPassword,
                    Port = options.Value.RabbitPort,
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQClient init fail");
            }
            _logger = logger;
        }
        public virtual void PushMessage(string routingKey, object message)
        {
            _logger.LogInformation($"PushMessage,routingKey:{routingKey}");
            _channel.QueueDeclare(queue: "message",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            _channel.BasicPublish(exchange: "message",
                                    routingKey: routingKey,
                                    basicProperties: null,
                                    body: body);
        }
    }
}
