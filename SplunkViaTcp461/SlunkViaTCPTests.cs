using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Splunk;


namespace SplunkViaTcp461
{
    [TestClass]
    public class SlunkViaTCPTests
    {
        private const string SplunkHost = "ubuntusrv-19-001";
        private const string SplunkPort = "8088";
        //private const string HecToken = "2ede6cea-de9b-4694-a5cf-e25353bd8196";
        private const string HecToken = "257a2c13-3336-4d70-8e04-6fb4f5f98fa5";

        [TestMethod]
        public void CanLogToSplunk()
        {
            using (var logger = new LoggerConfiguration()
                .WriteTo.EventCollector($"http://{SplunkHost}:{SplunkPort}/services/collector", HecToken)
                .CreateLogger())
            {
                logger.Warning($"Dummy message sent @ {DateTime.Now:O}");
            }
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
            var handler = new HttpClientHandler();
            
            var logger =  new LoggerConfiguration()
                .WriteTo.EventCollector($"http://{SplunkHost}:{SplunkPort}/services/collector", HecToken).CreateLogger();
            

            Console.WriteLine("Release breakpoint once the ip address is renewed");
            System.Diagnostics.Debugger.Break();


            logger.Warning($"Dummy message sent @ {DateTime.Now:O}");
            
            logger.Dispose();
        }

    }
}


/*
 * 
 * docker run --network splunkNet --name splunkIndexer --hostname splunkIndexer -p 8000:8000 -p 8088:8088 -e "SPLUNK_PASSWORD=splunkPwd" -e "SPLUNK_START_ARGS=--accept-license" -it splunk/splunk:latest
 * docker run --network splunkNet -p 1042:1042 --name splunkForwarder --hostname splunkForwarder -e "SPLUNK_PASSWORD=splunkPwd" -e "SPLUNK_START_ARGS=--accept-license" -e "SPLUNK_STANDALONE_URL=splunkIndexer" -it splunk/universalforwarder:latest
 * 
 */
