using System;

namespace ConfigManager.Core.MessageBroker
{
    public interface IMessageBrokerClient
    {
        void DeclareExchange(string exchange);
        void DeclareQueue(string queueName, string exchange, string routingKey,bool autoDelete);
        void Publish(string message, string exchange, string routingKey);
        void Subscribe(string queueName, Action<string> onMessageReceived);
    }
}