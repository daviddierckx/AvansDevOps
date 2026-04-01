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
    }
}
