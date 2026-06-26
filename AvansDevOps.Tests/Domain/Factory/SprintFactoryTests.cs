using System;
using AvansDevOps.Domain.Factory;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Pipelines;
using AvansDevOps.Domain.Strategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Factory
{
    [TestClass]
    public class SprintFactoryTests
    {
        // TC-26: ReviewSprintFactory produceert altijd een ReviewSprint
        [TestMethod]
        public void ReviewSprintFactory_CreateSprint_ReturnsReviewSprint()
        {
            SprintFactory factory = new ReviewSprintFactory();
            Sprint sprint = factory.CreateSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Assert.IsInstanceOfType(sprint, typeof(ReviewSprint));
        }

        // TC-27: ReleaseSprintFactory produceert altijd een ReleaseSprint
        [TestMethod]
        public void ReleaseSprintFactory_CreateSprint_ReturnsReleaseSprint()
        {
            SprintFactory factory = new ReleaseSprintFactory();
            Sprint sprint = factory.CreateSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Assert.IsInstanceOfType(sprint, typeof(ReleaseSprint));
        }

        // TC-28: Aangemaakte sprint begint in Created-state
        [TestMethod]
        public void ReviewSprintFactory_CreateSprint_StartsInCreatedState()
        {
            SprintFactory factory = new ReviewSprintFactory();
            Sprint sprint = factory.CreateSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Assert.AreEqual(SprintStatus.CREATED, sprint.Status);
        }

        // TC-29: Factory respecteert polymorfisme — sprint is bruikbaar via abstracte Sprint-interface
        [TestMethod]
        public void Factory_ReturnedSprint_CanBeUsedPolymorphically()
        {
            SprintFactory factory = new ReleaseSprintFactory();
            Sprint sprint = factory.CreateSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();
            Assert.AreEqual(SprintStatus.ACTIVE, sprint.Status);
        }

        [TestMethod]
        public void ReviewSprint_CloseWithoutDocument_ThrowsException()
        {
            ReviewSprint sprint = new ReviewSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();
            sprint.Finish();
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Close());
        }

        [TestMethod]
        public void ReviewSprint_CloseWithDocument_BecomesClosedState()
        {
            ReviewSprint sprint = new ReviewSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();
            sprint.Finish();
            sprint.UploadReviewDocument();
            sprint.Close();
            Assert.AreEqual(SprintStatus.CLOSED, sprint.Status);
        }

        [TestMethod]
        public void ReleaseSprint_ReleaseWithoutPipeline_ThrowsException()
        {
            ReleaseSprint sprint = new ReleaseSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            sprint.Start();
            sprint.Finish();
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Release());
        }

        [TestMethod]
        public void ReleaseSprint_ReleaseWithSuccessfulPipeline_BecomesReleased()
        {
            ReleaseSprint sprint = new ReleaseSprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
            Pipeline pipeline = new Pipeline("Release Pipeline");
            pipeline.AddAction(new BuildStrategy());
            sprint.SetPipeline(pipeline);
            sprint.Start();
            sprint.Finish();
            bool result = sprint.Release();
            Assert.IsTrue(result);
            Assert.AreEqual(SprintStatus.RELEASED, sprint.Status);
        }
    }
}
