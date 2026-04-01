using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.Observer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR6Tests
    {
        [TestMethod]
        public void TC6_1_NotificatieBijReadyForTesting()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager notificationManager = new NotificationManager();
            notificationManager.AddChannel(channel);

            Developer tester = new Developer("Test Tina", Role.TESTER);
            notificationManager.Subscribe(new TesterNotifier(new List<Developer> { tester }, notificationManager));

            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);
            Activity activity = new Activity("UI", "Maak login scherm");
            activity.Complete();
            item.AddActivity(activity);

            item.Doing();
            item.ReadyForTesting();

            Assert.AreEqual(1, channel.Sent.Count);
            StringAssert.Contains(channel.Sent[0].Message, "klaar voor testing");
            Assert.AreEqual("Test Tina", channel.Sent[0].Recipient);
        }

        [TestMethod]
        public void TC6_2_NotificatieBijFouten()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager notificationManager = new NotificationManager();
            notificationManager.AddChannel(channel);

            Developer developer = new Developer("Dev Daan", Role.DEVELOPER);
            Developer scrumMaster = new Developer("Scrum Sam", Role.SCRUMMASTER);

            notificationManager.Subscribe(new DeveloperNotifier(new List<Developer> { developer }, notificationManager));
            notificationManager.Subscribe(new ScrumMasterNotifier(scrumMaster, notificationManager));

            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);
            Activity activity = new Activity("UI", "Maak login scherm");
            activity.Complete();
            item.AddActivity(activity);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.ToDo();

            Assert.AreEqual(2, channel.Sent.Count);
            StringAssert.Contains(channel.Sent[0].Message, "teruggezet naar ToDo");
            StringAssert.Contains(channel.Sent[1].Message, "teruggezet naar ToDo");
        }

        [TestMethod]
        public void TC6_3_MeerdereNotificatiekanalenViaAdapter()
        {
            RecordingChannel channelOne = new RecordingChannel();
            RecordingChannel channelTwo = new RecordingChannel();

            NotificationManager notificationManager = new NotificationManager();
            notificationManager.AddChannel(channelOne);
            notificationManager.AddChannel(channelTwo);

            notificationManager.Notify("Testmelding", "Team");

            Assert.AreEqual(1, channelOne.Sent.Count);
            Assert.AreEqual(1, channelTwo.Sent.Count);
            Assert.AreEqual("Testmelding", channelOne.Sent[0].Message);
            Assert.AreEqual("Testmelding", channelTwo.Sent[0].Message);
        }

        private class RecordingChannel : INotificationChannel
        {
            public readonly List<RecordedNotification> Sent = new List<RecordedNotification>();

            public void Send(string message, string recipient)
            {
                Sent.Add(new RecordedNotification(message, recipient));
            }
        }

        private class RecordedNotification
        {
            public string Message { get; private set; }
            public string Recipient { get; private set; }

            public RecordedNotification(string message, string recipient)
            {
                Message = message;
                Recipient = recipient;
            }
        }
    }
}
