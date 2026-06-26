using System;
using System.IO;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Infrastructure
{
    [TestClass]
    public class NotificationObserverTests
    {
        private BacklogItem CreateItem()
        {
            return new BacklogItem("Login", "Gebruiker kan inloggen", 5, new NotificationManager());
        }

        [TestMethod]
        public void EmailNotificationSender_Update_SendsViaEmailChannel()
        {
            var channel = new EmailAdapter("smtp.test.com", 587, "user", "pass");
            var sender = new EmailNotificationSender("jan@test.com", channel);
            var item = CreateItem();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                sender.Update("ready_for_testing", item);
                StringAssert.Contains(sw.ToString(), "[EMAIL]");
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            }
        }

        [TestMethod]
        public void SlackNotificationSender_Update_SendsViaSlackChannel()
        {
            var channel = new SlackAdapter("https://hooks.slack.com/test", "C12345", "xoxb-token");
            var sender = new SlackNotificationSender("@jan", channel);
            var item = CreateItem();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                sender.Update("returned_to_todo", item);
                StringAssert.Contains(sw.ToString(), "[SLACK]");
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            }
        }

        [TestMethod]
        public void EmailNotificationSender_ImplementsIObserver()
        {
            var channel = new EmailAdapter("smtp.test.com", 587, "user", "pass");
            var sender = new EmailNotificationSender("jan@test.com", channel);
            Assert.IsInstanceOfType(sender, typeof(AvansDevOps.Domain.Observer.IObserver));
        }

        [TestMethod]
        public void SlackNotificationSender_ImplementsIObserver()
        {
            var channel = new SlackAdapter("https://hooks.slack.com/test", "C12345", "xoxb-token");
            var sender = new SlackNotificationSender("@jan", channel);
            Assert.IsInstanceOfType(sender, typeof(AvansDevOps.Domain.Observer.IObserver));
        }
    }
}
