namespace myclinic_back.Interfaces
{
    public interface ILogEventPublisher
    {
        Guid Subscribe(ILogObserver observer);
        void Unsubscribe(Guid id);
        Task PublishAsync(ILogObserver.LogEvent evt);
    }
}
