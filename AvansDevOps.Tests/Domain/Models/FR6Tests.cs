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

        [TestMethod]
        public void TC6_4_ReadyForTestingNaarTodoBrengtDeveloperNotificatie()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer dev = new Developer("Dev Jan", Role.DEVELOPER);
            nm.Subscribe(new DeveloperNotifier(new List<Developer> { dev }, nm));

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("X", "Y");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.ToDo(); // triggers returned_to_todo

            Assert.AreEqual(1, channel.Sent.Count);
            StringAssert.Contains(channel.Sent[0].Message, "teruggezet naar ToDo");
            Assert.AreEqual("Dev Jan", channel.Sent[0].Recipient);
        }

        [TestMethod]
        public void TC6_5_TesterNotifierNegeertAndereEvents()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer tester = new Developer("Tina", Role.TESTER);
            nm.Subscribe(new TesterNotifier(new List<Developer> { tester }, nm));

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            nm.NotifyObservers("returned_to_todo", item); // TesterNotifier should ignore this

            Assert.AreEqual(0, channel.Sent.Count);
        }

        [TestMethod]
        public void TC6_6_DeveloperNotifierNegeertAndereEvents()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer dev = new Developer("Dev Jan", Role.DEVELOPER);
            nm.Subscribe(new DeveloperNotifier(new List<Developer> { dev }, nm));

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            nm.NotifyObservers("ready_for_testing", item); // DeveloperNotifier should ignore this

            Assert.AreEqual(0, channel.Sent.Count);
        }

        [TestMethod]
        public void TC6_7_ProductOwnerObserverReageertOpSprintEvents()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer owner = new Developer("Product Owner Paul", Role.PRODUCTOWNER);
            nm.Subscribe(new ProductOwnerObserver(owner, nm));

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            nm.NotifyObservers("sprint_released", item);
            nm.NotifyObservers("sprint_cancelled", item);

            Assert.AreEqual(2, channel.Sent.Count);
            StringAssert.Contains(channel.Sent[0].Message, "sprint_released");
            StringAssert.Contains(channel.Sent[1].Message, "sprint_cancelled");
            Assert.AreEqual("Product Owner Paul", channel.Sent[0].Recipient);
        }

        [TestMethod]
        public void TC6_8_ProductOwnerObserverNegeertAndereEvents()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer owner = new Developer("Paul", Role.PRODUCTOWNER);
            nm.Subscribe(new ProductOwnerObserver(owner, nm));

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            nm.NotifyObservers("ready_for_testing", item);
            nm.NotifyObservers("returned_to_todo", item);

            Assert.AreEqual(0, channel.Sent.Count);
        }

        [TestMethod]
        public void TC6_9_ScrumMasterNotifierReageertOpReturnedToTodo()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer sm = new Developer("Sam", Role.SCRUMMASTER);
            nm.Subscribe(new ScrumMasterNotifier(sm, nm));

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            nm.NotifyObservers("returned_to_todo", item);

            Assert.AreEqual(1, channel.Sent.Count);
            Assert.AreEqual("Sam", channel.Sent[0].Recipient);
            StringAssert.Contains(channel.Sent[0].Message, "teruggezet naar ToDo");
        }

        [TestMethod]
        public void TC6_10_ObserverAfmeldenStoptNotificaties()
        {
            RecordingChannel channel = new RecordingChannel();
            NotificationManager nm = new NotificationManager();
            nm.AddChannel(channel);

            Developer tester = new Developer("Tina", Role.TESTER);
            TesterNotifier notifier = new TesterNotifier(new List<Developer> { tester }, nm);
            nm.Subscribe(notifier);

            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("X", "Y");
            a.Complete();
            item.AddActivity(a);
            item.Doing();
            item.ReadyForTesting(); // sends notification

            nm.Unsubscribe(notifier);
            item.ToDo();
            item.Doing();
            item.ReadyForTesting(); // should NOT notify because unsubscribed

            Assert.AreEqual(1, channel.Sent.Count);
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
