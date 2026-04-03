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

        [TestMethod]
        public void TC8_3_PipelineZonderActionsSuccesvol()
        {
            Pipeline pipeline = new Pipeline("Lege Pipeline");
            pipeline.Execute();

            Assert.IsTrue(pipeline.LastRunSuccessful);
            Assert.AreEqual(0, pipeline.Actions.Count);
        }

        [TestMethod]
        public void TC8_4_PipelineActionsZijnBeschikbaar()
        {
            Pipeline pipeline = new Pipeline("P");
            pipeline.AddAction(new BuildStrategy());
            pipeline.AddAction(new TestStrategy());

            Assert.AreEqual(2, pipeline.Actions.Count);
            Assert.AreEqual("Build", pipeline.Actions[0].GetStrategyName());
            Assert.AreEqual("Test", pipeline.Actions[1].GetStrategyName());
            Assert.AreEqual("Deploy", new DeployStrategy().GetStrategyName());
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
