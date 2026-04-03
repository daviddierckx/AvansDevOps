using System;
using System.IO;
using AvansDevOps.Domain.Factory;
using AvansDevOps.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Factory
{
    [TestClass]
    public class RapportFactoryTests
    {
        [TestMethod]
        public void BurndownChart_Generate_WrijftNaarConsole()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5));
            BurndownChart chart = new BurndownChart();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                chart.Generate(sprint);
                
                var output = sw.ToString();
                StringAssert.Contains(output, "[RAPPORT] Burndown chart voor sprint: Sprint 1");
                StringAssert.Contains(output, "Totaal items: 0");

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            Assert.AreEqual("BurndownChart", chart.GetStrategyName());
        }

        [TestMethod]
        public void EffortPointsChart_Generate_WrijftNaarConsole()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5));
            Developer dev = new Developer("Jan", Role.DEVELOPER);
            sprint.AddDeveloper(dev);
            
            var nm = new AvansDevOps.Domain.Notifications.NotificationManager();
            var item = new BacklogItem("Item 1", "Desc", 8, nm);
            item.AssignDeveloper(dev);
            sprint.AddBacklogItem(item);

            EffortPointsChart chart = new EffortPointsChart();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                chart.Generate(sprint);
                
                var output = sw.ToString();
                StringAssert.Contains(output, "[RAPPORT] Effort points chart voor sprint: Sprint 1");
                StringAssert.Contains(output, "Jan: 8 punten");

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            Assert.AreEqual("EffortPointsChart", chart.GetStrategyName());
        }

        [TestMethod]
        public void TeamCompChart_Generate_WrijftNaarConsole()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5));
            Developer sm = new Developer("ScrumSam", Role.SCRUMMASTER);
            Developer dev = new Developer("Jan", Role.DEVELOPER);
            sprint.SetScrumMaster(sm);
            sprint.AddDeveloper(dev);

            TeamCompChart chart = new TeamCompChart();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                chart.Generate(sprint);
                
                var output = sw.ToString();
                StringAssert.Contains(output, "[RAPPORT] Team samenstelling voor sprint: Sprint 1");
                StringAssert.Contains(output, "Scrum Master: ScrumSam");
                StringAssert.Contains(output, "Developer: Jan");

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            Assert.AreEqual("TeamCompChart", chart.GetStrategyName());
        }
    }
}
