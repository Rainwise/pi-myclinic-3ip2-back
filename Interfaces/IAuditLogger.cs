namespace myclinic_back.Interfaces
{
    public interface IAuditLogger
    {
        Task LogCrudAsync(string entity, string action, int entityId);
        Guid Subscribe(IAuditObserver observer);
        void Unsubscribe(Guid id);
    }
}
