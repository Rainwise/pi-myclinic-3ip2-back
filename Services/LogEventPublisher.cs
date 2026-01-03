using myclinic_back.Interfaces;
using System.Collections.Concurrent;
using static myclinic_back.Interfaces.ILogObserver;

namespace myclinic_back.Services
{
    public class LogEventPublisher : ILogEventPublisher
    {
        private readonly ConcurrentDictionary<Guid, ILogObserver> _observers = new();

        public Guid Subscribe(ILogObserver observer)
        {
            var id = Guid.NewGuid();
            _observers.TryAdd(id, observer);
            return id;
        }

        public void Unsubscribe(Guid id) => _observers.TryRemove(id, out _);

        public async Task PublishAsync(LogEvent evt)
        {
            foreach (var o in _observers.Values)
            {
                await o.OnLogAsync(evt);
            }
        }
    }
}
