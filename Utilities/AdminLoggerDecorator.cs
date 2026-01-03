using myclinic_back.Interfaces;
using System.Security.Claims;

namespace myclinic_back.Utilities
{
    public class AdminLoggerDecorator : ICrudLogger
    {
        private readonly ICrudLogger _logger;
        private readonly IHttpContextAccessor _http;

        public AdminLoggerDecorator(ICrudLogger logger, IHttpContextAccessor http)
        {
            _logger = logger;
            _http = http;
        }

        public Task LogCrudAsync(string entity, string action, int entityId)
        {
            var user = _http.HttpContext?.User;

            //Console.WriteLine($"action={action}");
            //Console.WriteLine($"{user?.Identity?.IsAuthenticated}");

            var isAdmin = user?.Identity?.IsAuthenticated == true && user.IsInRole("Admin");

            //if (!isAdmin && user?.Identity?.IsAuthenticated == true)
            //{
            //    isAdmin = user.Claims.Any(c =>
            //        (c.Type == ClaimTypes.Role || c.Type == "role") &&
            //        string.Equals(c.Value, "Admin", StringComparison.OrdinalIgnoreCase));
            //}

            var decoratedAction = isAdmin ? $"ADMIN | {action}" : action;
            return _logger.LogCrudAsync(entity, decoratedAction, entityId);
        }
    }
}
