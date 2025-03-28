namespace QA.ClientInterface.Interfaces
{
    public interface IProcessMessage<T>
    {
        Task ProcessMessage(T body, CancellationToken stoppingToken);
    }
}

