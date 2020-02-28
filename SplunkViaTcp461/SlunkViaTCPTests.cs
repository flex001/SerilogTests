using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Sinks.Splunk;

namespace SplunkViaTcp461
{
    [TestClass]
    public class SlunkViaTCPTests
    {
        [TestMethod]
        public void CanLogToSplunk()
        {
            var ip = Dns.GetHostAddresses("vs-syslogelogdev.corp.belgrid.net").First();
            var splunkTcpSinkConnectionInfo = new SplunkTcpSinkConnectionInfo(ip, 514);
            var loggerConfiguration = new LoggerConfiguration().WriteTo.SplunkViaTcp(splunkTcpSinkConnectionInfo);
            var logger = loggerConfiguration.CreateLogger();
            logger.Warning($"Dummy message sent @ {DateTime.Now:O}");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        [Timeout(10000)]
        public void AnExceptionIsThrownWhenNoSplunk()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var splunkTcpSinkConnectionInfo = new SplunkTcpSinkConnectionInfo(ip, 514);
            var loggerConfiguration = new LoggerConfiguration().WriteTo.SplunkViaTcp(splunkTcpSinkConnectionInfo);
            var logger = loggerConfiguration.CreateLogger();
            logger.Warning($"Dummy message sent @ {DateTime.Now:O}");
        }

    }
}
