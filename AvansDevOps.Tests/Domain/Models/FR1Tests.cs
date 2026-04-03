using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR1Tests
    {
        [TestMethod]
        public void TC1_1_ProjectCanBeCreatedModifiedAndRetrieved()
        {
            Project project = new Project("Avans DevOps");

            Assert.AreEqual("Avans DevOps", project.Name);

            project.Name = "Avans DevOps v2";

            string retrievedName = project.Name;
            Assert.AreEqual("Avans DevOps v2", retrievedName);
        }

        [TestMethod]
        public void TC1_2_CreateProjectWithEmptyName_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Project(""));
            Assert.ThrowsException<ArgumentException>(() => new Project("   "));
        }

        [TestMethod]
        public void TC1_3_ProjectSprintsEnBacklogStartenLeeg()
        {
            Project project = new Project("Test Project");

            Assert.AreEqual(0, project.Sprints.Count);
            Assert.AreEqual(0, project.Backlog.Count);
        }

        [TestMethod]
        public void TC1_4_SprintToevoegenAanProject()
        {
            Project project = new Project("Test Project");
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));

            project.AddSprint(sprint);

            Assert.AreEqual(1, project.Sprints.Count);
            Assert.AreSame(sprint, project.Sprints[0]);
        }

        [TestMethod]
        public void TC1_5_BacklogItemToevoegenAanProject()
        {
            Project project = new Project("Test Project");
            NotificationManager nm = new NotificationManager();
            BacklogItem item = new BacklogItem("Item 1", "Omschrijving", 3, nm);

            project.AddBacklogItem(item);

            Assert.AreEqual(1, project.Backlog.Count);
            Assert.AreSame(item, project.Backlog[0]);
        }

        [TestMethod]
        public void TC1_6_MeerdereSprintsEnBacklogItemsToevoegenAanProject()
        {
            Project project = new Project("Test Project");
            NotificationManager nm = new NotificationManager();

            project.AddSprint(new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14)));
            project.AddSprint(new Sprint("Sprint 2", DateTime.Today.AddDays(15), DateTime.Today.AddDays(28)));
            project.AddBacklogItem(new BacklogItem("Item A", "Desc A", 2, nm));
            project.AddBacklogItem(new BacklogItem("Item B", "Desc B", 5, nm));

            Assert.AreEqual(2, project.Sprints.Count);
            Assert.AreEqual(2, project.Backlog.Count);
        }
    }
}
