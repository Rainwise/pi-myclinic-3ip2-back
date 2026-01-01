namespace myclinic_back.Interfaces
{
    public interface ILogObserver
    {
        public record LogEvent(DateTime UtcTime, string Message);

        Task OnLogAsync(LogEvent logEvent);
    }
}
