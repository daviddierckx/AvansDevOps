using System;
using AvansDevOps.Domain.Models;
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
    }
}
