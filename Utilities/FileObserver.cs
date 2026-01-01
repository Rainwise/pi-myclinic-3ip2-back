using myclinic_back.Interfaces;
using System.Text;
using static myclinic_back.Interfaces.ILogObserver;

namespace myclinic_back.Utilities
{
    public class FileObserver : ILogObserver
    {
        private const string FilePath =
                    @"C:\Users\Korisnik\Desktop\PI_Novo\pi-myclinic-3ip2-back\ObserverLog.txt";

        //private static readonly SemaphoreSlim _lock = new(1, 1);

        public async Task OnLogAsync(LogEvent auditEvent)
        {
            var dir = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            var line = $"{auditEvent.UtcTime:o} | {auditEvent.Message}{Environment.NewLine}";

            //await _lock.WaitAsync();
            //try
            //{
                await File.AppendAllTextAsync(FilePath, line, Encoding.UTF8);
            //}
            //finally
            //{
            //    _lock.Release();
            //}
        }
    }
}
