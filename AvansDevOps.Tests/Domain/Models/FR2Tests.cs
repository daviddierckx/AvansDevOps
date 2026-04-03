using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR2Tests
    {
        [TestMethod]
        public void TC2_1_BacklogItemAanmakenEnKoppelenAanSprint()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem backlogItem = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            sprint.AddBacklogItem(backlogItem);

            Assert.AreEqual(1, sprint.Backlog.Count);
            Assert.AreSame(backlogItem, sprint.Backlog[0]);
        }

        [TestMethod]
        public void TC2_2_DeveloperToewijzenEnLimietValideren_MaximaalEenDeveloperPerBacklogItem()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem backlogItem = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Developer firstDeveloper = new Developer("Jan", Role.DEVELOPER);
            Developer secondDeveloper = new Developer("Piet", Role.DEVELOPER);

            backlogItem.AssignDeveloper(firstDeveloper);
            backlogItem.AssignDeveloper(secondDeveloper);

            Assert.AreSame(secondDeveloper, backlogItem.Developer);
            Assert.AreNotSame(firstDeveloper, backlogItem.Developer);
        }

        [TestMethod]
        public void TC2_3_BacklogItemAanmakenZonderDeveloper_DeveloperIsNull()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem backlogItem = new BacklogItem("Feature", "Beschrijving", 3, notificationManager);

            Assert.IsNull(backlogItem.Developer);
        }

        [TestMethod]
        public void TC2_4_BacklogItemEigenschappenCorrectOpgeslagen()
        {
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem backlogItem = new BacklogItem("Login", "Gebruiker kan inloggen", 8, notificationManager);

            Assert.AreEqual("Login", backlogItem.Name);
            Assert.AreEqual("Gebruiker kan inloggen", backlogItem.Description);
            Assert.AreEqual(8, backlogItem.EffortPoints);
            Assert.AreEqual(8, backlogItem.GetEffortPoints());
        }

        [TestMethod]
        public void TC2_5_MeerdereBacklogItemsAanSprintKoppelen()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            NotificationManager nm = new NotificationManager();
            BacklogItem item1 = new BacklogItem("Feature A", "Desc A", 3, nm);
            BacklogItem item2 = new BacklogItem("Feature B", "Desc B", 5, nm);

            sprint.AddBacklogItem(item1);
            sprint.AddBacklogItem(item2);

            Assert.AreEqual(2, sprint.Backlog.Count);
        }

        [TestMethod]
        public void TC2_6_BacklogItemVerwijderenUitSprintVoorStart()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);

            sprint.AddBacklogItem(item);
            sprint.RemoveBacklogItem(item);

            Assert.AreEqual(0, sprint.Backlog.Count);
        }

        [TestMethod]
        public void TC2_7_BacklogItemVerwijderenTijdensActieveSprint_ThrowsException()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Feature", "Desc", 3, nm);

            sprint.AddBacklogItem(item);
            sprint.Start();

            Assert.ThrowsException<InvalidOperationException>(() => sprint.RemoveBacklogItem(item));
        }
    }
}
