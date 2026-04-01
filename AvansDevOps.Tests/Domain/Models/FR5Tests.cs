using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR5Tests
    {
        [TestMethod]
        public void TC5_1_SprintAanmakenMetDatums()
        {
            DateTime startDate = new DateTime(2026, 4, 1);
            DateTime endDate = new DateTime(2026, 4, 15);

            Sprint sprint = new Sprint("Sprint 1", startDate, endDate);

            Assert.AreEqual(startDate, sprint.StartDate);
            Assert.AreEqual(endDate, sprint.EndDate);
            Assert.AreEqual(SprintStatus.CREATED, sprint.Status);
        }

        [TestMethod]
        public void TC5_2_SprintWijzigenTijdensUitvoeringBlokkeren()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();

            Pipeline pipeline = new Pipeline("Release Pipeline");
            NotificationManager notificationManager = new NotificationManager();
            BacklogItem backlogItem = new BacklogItem("Login", "Gebruiker kan inloggen", 5, notificationManager);

            Assert.ThrowsException<InvalidOperationException>(() => sprint.SetPipeline(pipeline));
            Assert.ThrowsException<InvalidOperationException>(() => sprint.AddBacklogItem(backlogItem));
        }

        [TestMethod]
        public void TC5_3_SprintAutomatischAfrondenNaEinddatum()
        {
            DateTime now = DateTime.Now;
            Sprint sprint = new Sprint("Verlopen sprint", now.AddDays(-10), now.AddDays(-1));

            sprint.Start();

            Assert.AreEqual(SprintStatus.FINISHED, sprint.Status);
        }

    }
}
