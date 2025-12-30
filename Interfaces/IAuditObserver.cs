namespace myclinic_back.Interfaces
{
    public interface IAuditObserver
    {
        public record AuditEvent(DateTime UtcTime, string Message);

        Task OnAuditAsync(AuditEvent auditEvent);
    }
}
