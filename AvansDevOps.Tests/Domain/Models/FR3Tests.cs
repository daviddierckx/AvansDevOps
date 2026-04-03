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

        [TestMethod]
        public void TC3_3_BacklogItemZonderActiviteiten_AllActivitiesDoneIsTrue()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Geen activiteiten", 5, notificationManager);

            Assert.IsTrue(item.AllActivitiesDone());
        }

        [TestMethod]
        public void TC3_4_ActiviteitToevoegenNaDoneItemWordtGeweigerd()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Activity activity = new Activity("UI", "Maak login scherm");
            activity.Complete();
            item.AddActivity(activity);
            item.Doing();
            item.ReadyForTesting();
            item.Testing();
            item.Tested();
            item.Done();

            Assert.ThrowsException<InvalidOperationException>(() =>
                item.AddActivity(new Activity("Nieuweactiviteit", "Beschrijving")));
        }

        [TestMethod]
        public void TC3_5_ActivityStatusBijhouden()
        {
            Activity activity = new Activity("UI", "Maak scherm");

            Assert.AreEqual("Todo", activity.GetStatus());
            Assert.IsFalse(activity.IsDone());

            activity.Complete();

            Assert.AreEqual("Done", activity.GetStatus());
            Assert.IsTrue(activity.IsDone());
            Assert.AreEqual(0, activity.GetEffortPoints());
        }

        [TestMethod]
        public void TC3_6_MeerdereActiviteitenGedeeltelijkVoltooid_AllActivitiesDoneIsFalse()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Beschrijving", 3, notificationManager);

            Activity a1 = new Activity("Stap 1", "Doe iets");
            Activity a2 = new Activity("Stap 2", "Doe nog iets");
            item.AddActivity(a1);
            item.AddActivity(a2);
            a1.Complete();

            Assert.IsFalse(item.AllActivitiesDone());
        }
    }
}
