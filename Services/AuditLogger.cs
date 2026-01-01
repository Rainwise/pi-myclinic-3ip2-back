using myclinic_back.Interfaces;
using myclinic_back.Models;
using System.Collections.Concurrent;
using static myclinic_back.Interfaces.IAuditObserver;

namespace myclinic_back.Services
{
    public class AuditLogger : IAuditLogger
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConcurrentDictionary<Guid, IAuditObserver> _observers = new();

        public AuditLogger(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Guid Subscribe(IAuditObserver observer)
        {
            var id = Guid.NewGuid();
            _observers.TryAdd(id, observer);
            return id;
        }

        public void Unsubscribe(Guid id) => _observers.TryRemove(id, out _);

        public async Task LogCrudAsync(string entity, string action, int entityId)
        {
            var idPart = $" (Id={entityId})";
            var message = $"{action} {entity}{idPart}";

            using (var scope = _scopeFactory.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<PiProjectContext>();

                ctx.Logs.Add(new Log
                {
                    Message = message,
                    Timestamp = DateTime.Now.ToLocalTime().AddHours(1),
                });

                await ctx.SaveChangesAsync();
            }

            var evt = new AuditEvent(DateTime.UtcNow, message);

            foreach (var o in _observers.Values)
            {
                try
                {
                    await o.OnAuditAsync(evt);
                }
                catch
                {

                }
            }
        }

        public IServiceScopeFactory ScopeFactory => _scopeFactory;
    }
}
