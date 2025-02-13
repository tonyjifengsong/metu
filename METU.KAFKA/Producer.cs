using Confluent.Kafka;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
 public   class Producer : IDisposable
    {
        /// <summary>
        /// 负责生成producer
        /// </summary>
        ProducerBuilder<string, object> builder;
        ConcurrentQueue<IProducer<string, object>> producers;
        bool disposed = false;

        /// <summary>
        /// kafka服务节点
        /// </summary>
        public string BootstrapServers { get; private set; }
        /// <summary>
        /// Flush超时时间(ms)
        /// </summary>
        public int FlushTimeOut { get; set; } = 10000;
        /// <summary>
        /// 保留发布者数
        /// </summary>
        public int InitializeCount { get; set; } = 5;
        /// <summary>
        /// 默认的消息键值
        /// </summary>
        public string DefaultKey { get; set; }
        /// <summary>
        /// 默认的主题
        /// </summary>
        public string DefaultTopic { get; set; }
        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<object, Exception> ErrorHandler;
        /// <summary>
        /// 统计事件
        /// </summary>
        public event Action<object, string> StatisticsHandler;
        /// <summary>
        /// 日志事件
        /// </summary>
        public event Action<object, KafkaLogMessage> LogHandler;

        public Producer(params string[] bootstrapServers)
        {
            if (bootstrapServers == null || bootstrapServers.Length == 0)
            {
                throw new Exception("at least one server must be assigned");
            }

            this.BootstrapServers = string.Join(",", bootstrapServers);
            producers = new ConcurrentQueue<IProducer<string, object>>();
        }

        #region Private
        /// <summary>
        /// producer构造器
        /// </summary>
        /// <returns></returns>
        private ProducerBuilder<string, object> CreateProducerBuilder()
        {
            if (builder == null)
            {
                lock (this)
                {
                    if (builder == null)
                    {
                        ProducerConfig config = new ProducerConfig();
                        config.BootstrapServers = BootstrapServers;

                        //var config = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("bootstrap.servers", BootstrapServers) };

                        builder = new ProducerBuilder<string, object>(config);
                        Action<Delegate, object> tryCatchWrap = (@delegate, arg) =>
                        {
                            try
                            {
                                @delegate?.DynamicInvoke(arg);
                            }
                            catch { }
                        };
                        builder.SetErrorHandler((p, e) => tryCatchWrap(ErrorHandler, new Exception(e.Reason)));
                        builder.SetStatisticsHandler((p, e) => tryCatchWrap(StatisticsHandler, e));
                        builder.SetLogHandler((p, e) => tryCatchWrap(LogHandler, new KafkaLogMessage(e)));
                        builder.SetValueSerializer(new KafkaConverter());
                    }
                }
            }

            return builder;
        }
        /// <summary>
        /// 租赁一个发布者
        /// </summary>
        /// <returns></returns>
        private IProducer<string, object> RentProducer()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Producer));
            }

            IProducer<string, object> producer;
            lock (producers)
            {
                if (!producers.TryDequeue(out producer) || producer == null)
                {
                    CreateProducerBuilder();
                    producer = builder.Build();
                }
            }
            return producer;
        }
        /// <summary>
        /// 返回保存发布者
        /// </summary>
        /// <param name="producer"></param>
        private void ReturnProducer(IProducer<string, object> producer)
        {
            if (disposed) return;

            lock (producers)
            {
                if (producers.Count < InitializeCount && producer != null)
                {
                    producers.Enqueue(producer);
                }
                else
                {
                    producer?.Dispose();
                }
            }
        }
        #endregion

        #region Publish
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void PublishWithKey(string key, object message, Action<DeliveryResult> callback = null)
        {
            Publish(DefaultTopic, null, key, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void PublishWithKey(int? partition, string key, object message, Action<DeliveryResult> callback = null)
        {
            Publish(DefaultTopic, partition, key, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void PublishWithKey(string topic, string key, object message, Action<DeliveryResult> callback = null)
        {
            Publish(topic, null, key, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Publish(object message, Action<DeliveryResult> callback = null)
        {
            Publish(DefaultTopic, null, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Publish(int? partition, object message, Action<DeliveryResult> callback = null)
        {
            Publish(DefaultTopic, partition, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Publish(string topic, object message, Action<DeliveryResult> callback = null)
        {
            Publish(topic, null, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Publish(string topic, int? partition, object message, Action<DeliveryResult> callback = null)
        {
            Publish(topic, partition, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Publish(string topic, int? partition, string key, object message, Action<DeliveryResult> callback = null)
        {
            Publish(new KafkaMessage() { Key = key, Message = message, Partition = partition, Topic = topic }, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="kafkaMessage"></param>
        /// <param name="callback"></param>
        public void Publish(KafkaMessage kafkaMessage, Action<DeliveryResult> callback = null)
        {
            if (string.IsNullOrEmpty(kafkaMessage.Topic))
            {
                throw new ArgumentException("topic can not be empty", nameof(kafkaMessage.Topic));
            }
            if (string.IsNullOrEmpty(kafkaMessage.Key))
            {
                throw new ArgumentException("key can not be empty", nameof(kafkaMessage.Key));
            }

            var producer = RentProducer();
            if (kafkaMessage.Partition == null)
            {
                producer.Produce(kafkaMessage.Topic, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message }, dr => callback?.Invoke(new DeliveryResult(dr)));
            }
            else
            {
                var topicPartition = new TopicPartition(kafkaMessage.Topic, new Partition(kafkaMessage.Partition.Value));
                producer.Produce(topicPartition, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message }, dr => callback?.Invoke(new DeliveryResult(dr)));
            }

            producer.Flush(TimeSpan.FromMilliseconds(FlushTimeOut));

            ReturnProducer(producer);
        }
        #endregion

        #region PublishAsync
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishWithKeyAsync(string key, object message)
        {
            return await PublishAsync(DefaultTopic, null, key, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishWithKeyAsync(int? partition, string key, object message)
        {
            return await PublishAsync(DefaultTopic, partition, key, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishWithKeyAsync(string topic, string key, object message)
        {
            return await PublishAsync(topic, null, key, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishAsync(object message)
        {
            return await PublishAsync(DefaultTopic, null, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishAsync(int? partition, object message)
        {
            return await PublishAsync(DefaultTopic, partition, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishAsync(string topic, object message)
        {
            return await PublishAsync(topic, null, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> PublishAsync(string topic, int? partition, object message)
        {
            return await PublishAsync(topic, partition, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<DeliveryResult> PublishAsync(string topic, int? partition, string key, object message)
        {
            return await PublishAsync(new KafkaMessage() { Key = key, Message = message, Partition = partition, Topic = topic });
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="kafkaMessage"></param>
        /// <returns></returns>
        public async Task<DeliveryResult> PublishAsync(KafkaMessage kafkaMessage)
        {
            if (string.IsNullOrEmpty(kafkaMessage.Topic))
            {
                throw new ArgumentException("topic can not be empty", nameof(kafkaMessage.Topic));
            }
            if (string.IsNullOrEmpty(kafkaMessage.Key))
            {
                throw new ArgumentException("key can not be empty", nameof(kafkaMessage.Key));
            }

            var producer = RentProducer();
            DeliveryResult<string, object> deliveryResult;
            if (kafkaMessage.Partition == null)
            {
                deliveryResult = await producer.ProduceAsync(kafkaMessage.Topic, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message });
            }
            else
            {
                var topicPartition = new TopicPartition(kafkaMessage.Topic, new Partition(kafkaMessage.Partition.Value));
                deliveryResult = await producer.ProduceAsync(topicPartition, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message });
            }

            producer.Flush(new TimeSpan(0, 0, 0, 0, FlushTimeOut));

            ReturnProducer(producer);

            return new DeliveryResult(deliveryResult);
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            disposed = true;
            while (producers.Count > 0)
            {
                IProducer<string, object> producer;
                producers.TryDequeue(out producer);
                producer?.Dispose();
            }
            GC.Collect();
        }

        public static Producer Create(KafkaBaseOptions kafkaBaseOptions)
        {
            return new Producer(kafkaBaseOptions.BootstrapServers);
        }
        public override string ToString()
        {
            return BootstrapServers;
        }
    }

  
   
}
