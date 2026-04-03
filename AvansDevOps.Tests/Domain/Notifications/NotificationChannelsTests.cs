using System;
using System.IO;
using AvansDevOps.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Notifications
{
    [TestClass]
    public class NotificationChannelsTests
    {
        [TestMethod]
        public void EmailAdapter_Send_WrijftNaarConsole()
        {
            var emailAdapter = new EmailAdapter("smtp.test.com", 25, "user", "pass");

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                emailAdapter.Send("Test Bericht", "dev@test.com");
                
                var output = sw.ToString();
                StringAssert.Contains(output, "Verbinding met smtp.test.com:25");
                StringAssert.Contains(output, "[EMAIL] Aan: dev@test.com | Test Bericht");
                StringAssert.Contains(output, "Verbinding verbroken");

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
        }

        [TestMethod]
        public void SlackAdapter_Send_WrijftNaarConsole()
        {
            var slackAdapter = new SlackAdapter("http://webhook.slack", "channel123", "token_abc");

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                slackAdapter.Send("Test Slack", "dev");
                slackAdapter.ValidateToken();
                
                var output = sw.ToString();
                StringAssert.Contains(output, "Verbinding met http://webhook.slack");
                StringAssert.Contains(output, "[SLACK] Kanaal: channel123 | Aan: dev | Test Slack");
                StringAssert.Contains(output, "Verbinding verbroken");
                StringAssert.Contains(output, "Token gevalideerd");

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
        }
    }
}
