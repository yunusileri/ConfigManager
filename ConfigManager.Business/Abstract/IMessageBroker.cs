namespace ConfigManager.Business.Abstract;

public interface IMessageBroker
{
    void SendMessage(string message);
}