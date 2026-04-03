using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR4Tests
    {
        [TestMethod]
        public void TC4_1_InitieleStatusTodoValideren()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Assert.AreEqual("ToDo", item.GetStatus());
        }

        [TestMethod]
        public void TC4_2_GeldigeStatusOvergangenTesten()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Activity activity = new Activity("UI", "Maak login scherm");
            activity.Complete();
            item.AddActivity(activity);

            item.Doing();
            Assert.AreEqual("Doing", item.GetStatus());

            item.ReadyForTesting();
            Assert.AreEqual("ReadyForTesting", item.GetStatus());

            item.Testing();
            Assert.AreEqual("Testing", item.GetStatus());

            item.Tested();
            Assert.AreEqual("Tested", item.GetStatus());

            item.Done();
            Assert.AreEqual("Done", item.GetStatus());
        }

        [TestMethod]
        public void TC4_3_OngeldigeStatusOvergangenWeigeren()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Assert.ThrowsException<InvalidOperationException>(() => item.Done());
            Assert.AreEqual("ToDo", item.GetStatus());
        }

        [TestMethod]
        public void TC4_4_TodoNaarTodoIsOngeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);

            Assert.ThrowsException<InvalidOperationException>(() => item.ToDo());
        }

        [TestMethod]
        public void TC4_5_TodoNaarTestingEnTestedIsOngeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);

            Assert.ThrowsException<InvalidOperationException>(() => item.Testing());
            Assert.ThrowsException<InvalidOperationException>(() => item.Tested());
            Assert.ThrowsException<InvalidOperationException>(() => item.ReadyForTesting());
        }

        [TestMethod]
        public void TC4_6_DoingNaarTodoIsOngeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            item.Doing();

            Assert.ThrowsException<InvalidOperationException>(() => item.ToDo());
        }

        [TestMethod]
        public void TC4_7_DoingNaarDoingIsOngeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            item.Doing();

            Assert.ThrowsException<InvalidOperationException>(() => item.Doing());
        }

        [TestMethod]
        public void TC4_8_TestingNaarReadyForTestingIsGeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.ReadyForTesting(); // back to RFT

            Assert.AreEqual("ReadyForTesting", item.GetStatus());
        }

        [TestMethod]
        public void TC4_9_TestedNaarReadyForTestingIsGeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.ReadyForTesting(); // back to RFT

            Assert.AreEqual("ReadyForTesting", item.GetStatus());
        }

        [TestMethod]
        public void TC4_10_TestedNaarTodoIsGeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.ToDo(); // back to ToDo

            Assert.AreEqual("ToDo", item.GetStatus());
        }

        [TestMethod]
        public void TC4_11_DoneNaarTodoIsGeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.Done();
            item.ToDo(); // reopen from done

            Assert.AreEqual("ToDo", item.GetStatus());
            Assert.IsFalse(item.IsDone());
        }

        [TestMethod]
        public void TC4_12_DoneNaarDoingEnAndereStatesMogenNiet()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.Done();

            Assert.ThrowsException<InvalidOperationException>(() => item.Doing());
            Assert.ThrowsException<InvalidOperationException>(() => item.ReadyForTesting());
            Assert.ThrowsException<InvalidOperationException>(() => item.Testing());
            Assert.ThrowsException<InvalidOperationException>(() => item.Tested());
            Assert.ThrowsException<InvalidOperationException>(() => item.Done());
        }

        [TestMethod]
        public void TC4_13_ReadyForTestingNaarTodoIsGeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.ToDo();

            Assert.AreEqual("ToDo", item.GetStatus());
        }

        [TestMethod]
        public void TC4_14_ReadyForTestingNaarReadyForTestingIsOngeldig()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();

            Assert.ThrowsException<InvalidOperationException>(() => item.ReadyForTesting());
        }

        [TestMethod]
        public void TC4_15_CanEditIsFalseWhenDone()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);
            Activity a = new Activity("A", "B");
            a.Complete();
            item.AddActivity(a);

            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.Done();

            Assert.IsFalse(item.CanEdit());
        }

        [TestMethod]
        public void TC4_16_CanEditIsTrueWhenNotDone()
        {
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);

            Assert.IsTrue(item.CanEdit());
        }
    }
}
