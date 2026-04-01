using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR7Tests
    {
        [TestMethod]
        public void TC7_1_DiscussieEnReactiesToevoegen()
        {
            DiscussionThread thread = new DiscussionThread("Discussie login flow");
            Developer owner = new Developer("Olivia", Role.PRODUCTOWNER);
            Developer developer = new Developer("Daan", Role.DEVELOPER);

            thread.AddMessage(new Message("Moeten we OAuth gebruiken?", owner));
            thread.AddMessage(new Message("Ja, dat is veiliger.", developer));

            Assert.AreEqual(2, thread.Messages.Count);
            Assert.AreEqual("Moeten we OAuth gebruiken?", thread.Messages[0].Content);
            Assert.AreEqual("Ja, dat is veiliger.", thread.Messages[1].Content);
        }

        [TestMethod]
        public void TC7_2_WijzigingenNaAfrondingBlokkeren()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);
            DiscussionThread thread = new DiscussionThread("Discussie login flow");
            item.AddDiscussionThread(thread);

            Activity activity = new Activity("UI", "Maak login scherm");
            activity.Complete();
            item.AddActivity(activity);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.Done();

            Assert.IsTrue(thread.IsLocked);
            Assert.ThrowsException<InvalidOperationException>(() =>
                item.AddDiscussionThread(new DiscussionThread("Nieuwe discussie")));
            Assert.ThrowsException<InvalidOperationException>(() =>
                thread.AddMessage(new Message("Nieuwe reactie", new Developer("Daan", Role.DEVELOPER))));
        }
    }
}
