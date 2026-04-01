using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Pipelines;
using AvansDevOps.Domain.Strategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class FR8Tests
    {
        [TestMethod]
        public void TC8_1_PipelineKoppelenEnStarten()
        {
            Sprint sprint = new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Pipeline pipeline = new Pipeline("Release Pipeline");
            pipeline.AddAction(new BuildStrategy());
            pipeline.AddAction(new TestStrategy());
            pipeline.AddAction(new DeployStrategy());

            sprint.SetPipeline(pipeline);
            sprint.Start();
            sprint.Pipeline.Execute();

            Assert.AreSame(pipeline, sprint.Pipeline);
            Assert.IsTrue(sprint.Pipeline.LastRunSuccessful);
        }

        [TestMethod]
        public void TC8_2_PipelineResultaatRegistreren_SuccesEnFout()
        {
            Pipeline successPipeline = new Pipeline("Success Pipeline");
            successPipeline.AddAction(new BuildStrategy());
            successPipeline.Execute();
            Assert.IsTrue(successPipeline.LastRunSuccessful);

            Pipeline failingPipeline = new Pipeline("Failing Pipeline");
            failingPipeline.AddAction(new ThrowingActionStrategy());
            failingPipeline.Execute();
            Assert.IsFalse(failingPipeline.LastRunSuccessful);
        }

        private class ThrowingActionStrategy : IActionStrategy
        {
            public void Execute()
            {
                throw new InvalidOperationException("Geforceerde fout voor test");
            }

            public string GetStrategyName()
            {
                return "ThrowingAction";
            }
        }
    }
}
