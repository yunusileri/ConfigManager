using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConfigManager.Core.MessageBroker
{
    public class RabbitMqClient : IMessageBrokerClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(string connectionString)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(connectionString) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void DeclareExchange(string exchange)
        {
            _channel.ExchangeDeclare(exchange: exchange, type: "direct");
        }

        public void DeclareQueue(string queueName, string exchange, string routingKey,bool autoDelete)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: autoDelete,
                arguments: null);
            _channel.QueueBind(queue: queueName, exchange: exchange, routingKey: routingKey);
        }

        public void Publish(string message, string exchange, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
        }

        public void Subscribe(string queueName, Action<string> onMessageReceived)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onMessageReceived(message);
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}