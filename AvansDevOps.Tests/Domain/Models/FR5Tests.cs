using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.Pipelines;
using AvansDevOps.Domain.Factory;
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

        [TestMethod]
        public void TC5_4_SprintStartenVanuitNietCreatedStatus_ThrowsException()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();

            Assert.ThrowsException<InvalidOperationException>(() => sprint.Start());
        }

        [TestMethod]
        public void TC5_5_SprintManueelAfronden()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();
            sprint.Finish();

            Assert.AreEqual(SprintStatus.FINISHED, sprint.Status);
        }

        [TestMethod]
        public void TC5_6_SprintFinishenZonderActiveStatus_ThrowsException()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));

            Assert.ThrowsException<InvalidOperationException>(() => sprint.Finish());
        }

        [TestMethod]
        public void TC5_7_ScrumMasterInstellen()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Developer scrumMaster = new Developer("Sam", Role.SCRUMMASTER);

            sprint.SetScrumMaster(scrumMaster);

            Assert.AreSame(scrumMaster, sprint.ScrumMaster);
        }

        [TestMethod]
        public void TC5_8_ScrumMasterInstellenMetVerkeerdeRol_ThrowsException()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Developer developer = new Developer("Jan", Role.DEVELOPER);

            Assert.ThrowsException<InvalidOperationException>(() => sprint.SetScrumMaster(developer));
        }

        [TestMethod]
        public void TC5_9_DeveloperToevoegenAanSprint()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Developer dev = new Developer("Jan", Role.DEVELOPER);

            sprint.AddDeveloper(dev);

            Assert.AreEqual(1, sprint.Developers.Count);
            Assert.AreSame(dev, sprint.Developers[0]);
        }

        [TestMethod]
        public void TC5_10_ChangeStatusTijdensActiveSprint_ThrowsException()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();

            Assert.ThrowsException<InvalidOperationException>(() => sprint.ChangeStatus(SprintStatus.FINISHED));
        }

        [TestMethod]
        public void TC5_11_GenerateRapportNaAfgerondenSprint_BurndownChart()
        {
            DateTime now = DateTime.Now;
            Sprint sprint = new Sprint("Sprint 1", now.AddDays(-10), now.AddDays(-1));
            sprint.Start(); // auto-finishes because end date is in the past

            IRapportFactory factory = new BurndownChart();
            sprint.GenerateRapport(factory); // should not throw
        }

        [TestMethod]
        public void TC5_12_GenerateRapportNietAfgerondenSprint_ThrowsException()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));

            IRapportFactory factory = new BurndownChart();

            Assert.ThrowsException<InvalidOperationException>(() => sprint.GenerateRapport(factory));
        }

        [TestMethod]
        public void TC5_13_GetDoneItemCount()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            NotificationManager nm = new NotificationManager();
            BacklogItem item1 = new BacklogItem("Item 1", "Desc", 3, nm);
            BacklogItem item2 = new BacklogItem("Item 2", "Desc", 5, nm);

            // Complete item1 fully
            Activity a = new Activity("A", "B");
            a.Complete();
            item1.AddActivity(a);
            item1.Doing();
            item1.ReadyForTesting();
            item1.Testing();
            item1.Tested();
            item1.Done();

            sprint.AddBacklogItem(item1);
            sprint.AddBacklogItem(item2);

            Assert.AreEqual(1, sprint.GetDoneItemCount());
        }
    }
}
