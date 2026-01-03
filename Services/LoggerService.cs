using myclinic_back.Interfaces;
using myclinic_back.Models;
using System.Collections.Concurrent;
using static myclinic_back.Interfaces.ILogObserver;

namespace myclinic_back.Services
{
    public class LoggerService : ICrudLogger
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogEventPublisher _publisher;

        public LoggerService(IServiceScopeFactory scopeFactory, ILogEventPublisher publisher)
        {
            _scopeFactory = scopeFactory;
            _publisher = publisher;
        }

        public async Task LogCrudAsync(string entity, string action, int entityId)
        {
            var message = $"{action} {entity} (Id={entityId})";
            var time = DateTime.Now.ToLocalTime();

            using (var scope = _scopeFactory.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<PiProjectContext>();
                ctx.Logs.Add(new Log { Message = message, Timestamp = time });
                await ctx.SaveChangesAsync();
            }

            await _publisher.PublishAsync(new LogEvent(time, message));
        }
    }
}
