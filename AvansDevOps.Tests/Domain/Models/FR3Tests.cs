using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR3Tests
    {
        [TestMethod]
        public void TC3_1_ActiviteitenToevoegenEnDonePasAlsAllesVoltooid()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Activity activity1 = new Activity("UI", "Maak login scherm");
            Activity activity2 = new Activity("API", "Maak endpoint");
            item.AddActivity(activity1);
            item.AddActivity(activity2);

            item.Doing();

            activity1.Complete();
            activity2.Complete();

            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.Done();

            Assert.AreEqual("Done", item.GetStatus());
            Assert.IsTrue(item.IsDone());
        }

        [TestMethod]
        public void TC3_2_DoneZonderVoltooideActiviteitenWordtGeweigerd()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Activity activity1 = new Activity("UI", "Maak login scherm");
            Activity activity2 = new Activity("API", "Maak endpoint");
            item.AddActivity(activity1);
            item.AddActivity(activity2);

            item.Doing();
            activity1.Complete();

            Assert.ThrowsException<InvalidOperationException>(() => item.ReadyForTesting());
            Assert.AreEqual("Doing", item.GetStatus());
            Assert.IsFalse(item.AllActivitiesDone());
        }
    }
}
