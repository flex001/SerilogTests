using System;
using System.Diagnostics;
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
        private const string SplunkHost = "ubuntusrv-19-001";
        private const int SplunkPort = 1042;

        [TestMethod]
        public void CanLogToSplunk()
        {
            var ip = Dns.GetHostAddresses(SplunkHost).First();
            var splunkTcpSinkConnectionInfo = new SplunkTcpSinkConnectionInfo(ip, SplunkPort);
            var loggerConfiguration = new LoggerConfiguration().WriteTo.SplunkViaTcp(splunkTcpSinkConnectionInfo);
            var logger = loggerConfiguration.CreateLogger();
            logger.Warning($"Dummy message sent @ {DateTime.Now:O}");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AnExceptionIsThrownWhenNoSplunk()
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Assert.Fail("Please attach your debugger, and once the breakpoint is reached, change the IP address of the splunk server");
            }

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.WriteLine(msg);
            });

            var ip = Dns.GetHostAddresses(SplunkHost).First();
            var splunkTcpSinkConnectionInfo = new SplunkTcpSinkConnectionInfo(ip, SplunkPort);
            var loggerConfiguration = new LoggerConfiguration().WriteTo.SplunkViaTcp(splunkTcpSinkConnectionInfo);
            
            var logger = loggerConfiguration.CreateLogger();
            
            
            Console.WriteLine("Release breakpoint once the ip address is renewed");
            System.Diagnostics.Debugger.Break();
            logger.Warning($"Dummy message sent @ {DateTime.Now:O}");
        }

    }
}
