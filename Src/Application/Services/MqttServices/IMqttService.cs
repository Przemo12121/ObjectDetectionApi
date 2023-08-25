namespace Application.Services.MqttServices;

public interface IMqttService
{
    void Enqueue(object message);
}