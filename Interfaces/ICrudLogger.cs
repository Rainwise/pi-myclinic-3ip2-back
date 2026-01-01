namespace myclinic_back.Interfaces
{
    public interface ICrudLogger
    {
        Task LogCrudAsync(string entity, string action, int entityId);
        Guid Subscribe(ILogObserver observer);
        void Unsubscribe(Guid id);
    }
}
