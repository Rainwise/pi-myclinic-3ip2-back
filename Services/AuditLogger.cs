using myclinic_back.Interfaces;
using myclinic_back.Models;
using System.Collections.Concurrent;
using static myclinic_back.Interfaces.ILogObserver;

namespace myclinic_back.Services
{
    public class AuditLogger : ICrudLogger
    {

        private readonly IServiceScopeFactory _scopeFactory;
        //Mapa koja sadrži sve evente koji su se pretplatili i njihove ideve u obliku 'guid', koji je dobar jer je jeftin, jedinstven i brz
        private readonly ConcurrentDictionary<Guid, ILogObserver> _observers = new();

        public AuditLogger(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        //ovo omogućuje vanjskom kodu da pozove privatni kod 'private readonly' iz ove metode i čita ga, ne i prepravlja
        public IServiceScopeFactory ScopeFactory => _scopeFactory;

        //generira subsrcibere te se poziva u program.csu
        public Guid Subscribe(ILogObserver observer)
        {
            var id = Guid.NewGuid();
            _observers.TryAdd(id, observer);
            return id;
        }

        //unsuskrajba
        public void Unsubscribe(Guid id) => _observers.TryRemove(id, out _);

        public async Task LogCrudAsync(string entity, string action, int entityId)
        {
            //kreira poruku za logiranje
            var idPart = $" (Id={entityId})";
            var message = $"{action} {entity}{idPart}";

            //kreira se mini DI kontejner koji služi kao okvir u kojem singleton koristi dbcontext
            using (var scope = _scopeFactory.CreateScope())
            {
                //iz definiranog scopea koristim uslugu piprojectcontexta
                var ctx = scope.ServiceProvider.GetRequiredService<PiProjectContext>();

                ctx.Logs.Add(new Log
                {
                    Message = message,
                    Timestamp = DateTime.Now.ToLocalTime(),
                });

                await ctx.SaveChangesAsync();
            }

            var evt = new LogEvent(DateTime.Now.ToLocalTime(), message);

            foreach (var o in _observers.Values)
            {
                try
                {
                    await o.OnLogAsync(evt);
                }
                catch
                {

                }
            }
        }

        
    }
}
