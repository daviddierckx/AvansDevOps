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
    }
}
