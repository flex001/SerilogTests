using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace SplunkPerformanceViaHec
{
    [TestClass]
    public class PerformanceViaHec
    {
        private const string SplunkHost = "ubuntusrv-19-001";
        private const string SplunkHecPort = "8088";
        private const int SplunkUdpTcpPort = 1042;
        private const string HecToken = "69998c2e-0e38-477a-9ccd-e2671def3b18";

        [TestMethod]
        [Timeout(60000)]
        public void LogViaTcpForSomeTime()
        {
            const string searchKey = "1_minuteAttempt01Hec";
            using (var logger = new LoggerConfiguration()
                .WriteTo.EventCollector($"http://{SplunkHost}:{SplunkHecPort}/services/collector", HecToken)
                .CreateLogger())
            {
                CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                TaskFactory tf = new TaskFactory(cts.Token);
                var logBackGround = tf.StartNew(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        logger.Warning($"{searchKey} Dummy message sent @ {DateTime.Now:O}");
                    }
                }, cts.Token);

                logBackGround.Wait(((int)TimeSpan.FromMinutes(1).TotalMilliseconds),cts.Token);
            }

        }
    }
}
