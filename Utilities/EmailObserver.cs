using myclinic_back.Interfaces;
using myclinic_back.Utilities.Service;
using static myclinic_back.Interfaces.ILogObserver;

namespace myclinic_back.Utilities
{
    public class EmailObserver : ILogObserver
    {
        private readonly EmailService _email;

        public EmailObserver(EmailService email)
        {
            _email = email;
        }

        public Task OnLogAsync(LogEvent logEvent)
        {
            _email.Send(
                toEmail: "franjo0330@gmail.com",
                messageText: logEvent.Message
            );

            return Task.CompletedTask;
        }
    }
}
