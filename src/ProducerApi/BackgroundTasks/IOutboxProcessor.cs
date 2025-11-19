namespace ProducerApi.BackgroundTasks;

public interface IOutboxProcessor
{
    Task ProcessOutboxMessagesAsync();
}