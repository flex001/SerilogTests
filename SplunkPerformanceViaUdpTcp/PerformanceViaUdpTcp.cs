using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Sinks.Splunk;

namespace SplunkPerformanceViaUdpTcp
{
    [TestClass]
    public class PerformanceViaUdpTcp
    {
        private const string SplunkHost = "ubuntusrv-19-001";
        private const string SplunkHecPort = "8088";
        private const int SplunkUdpTcpPort = 1042;


        [TestMethod]
        public void LogViaTcpForSomeTime()
        {
            const string searchKey = "1_minuteTcp";
            var ip = Dns.GetHostAddresses(SplunkHost).First();
            using (var logger = new LoggerConfiguration()
                .WriteTo.SplunkViaTcp(new SplunkTcpSinkConnectionInfo(ip, SplunkUdpTcpPort))
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

                logBackGround.Wait(cts.Token);
            }

        }

        [TestMethod]
        public void LogViaUdpForSomeTime()
        {
            const string searchKey = "1_minuteUdp";
            var ip = Dns.GetHostAddresses(SplunkHost).First();
            using (var logger = new LoggerConfiguration()
                .WriteTo.SplunkViaTcp(new SplunkTcpSinkConnectionInfo(ip, SplunkUdpTcpPort))
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

                logBackGround.Wait(cts.Token);
            }

        }

    }
}
