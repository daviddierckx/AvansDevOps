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

        [TestMethod]
        public void TC7_3_MessageMakenMetLegeContentWeigeren()
        {
            Developer dev = new Developer("Daan", Role.DEVELOPER);
            Assert.ThrowsException<ArgumentException>(() => new Message("", dev));
            Assert.ThrowsException<ArgumentException>(() => new Message("   ", dev));
        }

        [TestMethod]
        public void TC7_4_MessageMakenMetNullAuteurWeigeren()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Message("Test", null));
        }

        [TestMethod]
        public void TC7_5_MessageKleurenVolgensRol()
        {
            Developer po = new Developer("PO", Role.PRODUCTOWNER);
            Developer sm = new Developer("SM", Role.SCRUMMASTER);
            Developer tester = new Developer("Te", Role.TESTER);
            Developer dev = new Developer("De", Role.DEVELOPER);

            Message mPo = new Message("1", po);
            Message mSm = new Message("2", sm);
            Message mTester = new Message("3", tester);
            Message mDev = new Message("4", dev);

            Assert.AreEqual("#FFD700", mPo.GetColor()); // Gold
            Assert.AreEqual("#378ADD", mSm.GetColor()); // Blue
            Assert.AreEqual("#1D9E75", mTester.GetColor()); // Green
            Assert.AreEqual("#FFFFFF", mDev.GetColor()); // White
        }

        [TestMethod]
        public void TC7_6_MeerdereDiscussieThreads()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Item", "Desc", 3, nm);
            DiscussionThread thread1 = new DiscussionThread("T1");
            DiscussionThread thread2 = new DiscussionThread("T2");

            item.AddDiscussionThread(thread1);
            item.AddDiscussionThread(thread2);

            Assert.AreEqual(2, item.DiscussionThreads.Count);
        }

        [TestMethod]
        public void TC7_7_LockThreadsViaBacklogItem()
        {
            DiscussionThread thread1 = new DiscussionThread("T1");
            DiscussionThread thread2 = new DiscussionThread("T2");

            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Item", "Desc", 3, nm);
            item.AddDiscussionThread(thread1);
            item.AddDiscussionThread(thread2);

            item.LockThreads();

            Assert.IsTrue(thread1.IsLocked);
            Assert.IsTrue(thread2.IsLocked);
        }

        [TestMethod]
        public void TC7_8_UnlockThreadsBlijftGelocked()
        {
            // Per requirement, once locked due to done, they stay locked
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Item", "Desc", 3, nm);
            DiscussionThread thread1 = new DiscussionThread("T1");
            item.AddDiscussionThread(thread1);

            item.LockThreads();
            item.UnlockThreads();

            Assert.IsTrue(thread1.IsLocked); // Blijft true
        }

    }
}
