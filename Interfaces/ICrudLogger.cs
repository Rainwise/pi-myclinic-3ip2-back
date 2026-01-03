namespace myclinic_back.Interfaces
{
    public interface ICrudLogger
    {
        Task LogCrudAsync(string entity, string action, int entityId);
    }
}
