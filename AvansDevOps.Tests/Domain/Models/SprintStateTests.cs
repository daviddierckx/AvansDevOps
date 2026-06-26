using System;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class SprintStateTests
    {
        private Sprint NewSprint()
        {
            return new Sprint("Sprint 1", DateTime.Today, DateTime.Today.AddDays(14));
        }

        [TestMethod]
        public void Sprint_StartsIn_CreatedState()
        {
            Assert.AreEqual(SprintStatus.CREATED, NewSprint().Status);
        }

        [TestMethod]
        public void CreatedState_Start_BecomesActive()
        {
            var sprint = NewSprint();
            sprint.Start();
            Assert.AreEqual(SprintStatus.ACTIVE, sprint.Status);
        }

        [TestMethod]
        public void CreatedState_Finish_ThrowsException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => NewSprint().Finish());
        }

        [TestMethod]
        public void ActiveState_Finish_BecomesFinished()
        {
            var sprint = NewSprint();
            sprint.Start();
            sprint.Finish();
            Assert.AreEqual(SprintStatus.FINISHED, sprint.Status);
        }

        [TestMethod]
        public void ActiveState_Start_ThrowsException()
        {
            var sprint = NewSprint();
            sprint.Start();
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Start());
        }

        [TestMethod]
        public void FinishedState_Start_ThrowsException()
        {
            var sprint = NewSprint();
            sprint.Start();
            sprint.Finish();
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Start());
        }

        [TestMethod]
        public void FinishedState_Finish_ThrowsException()
        {
            var sprint = NewSprint();
            sprint.Start();
            sprint.Finish();
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Finish());
        }

        [TestMethod]
        public void ClosedState_Start_ThrowsException()
        {
            var sprint = NewSprint();
            sprint.Start();
            sprint.Finish();
            sprint.ChangeStatus(SprintStatus.CLOSED);
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Start());
        }

        [TestMethod]
        public void ReleasedState_Start_ThrowsException()
        {
            var sprint = NewSprint();
            sprint.Start();
            sprint.Finish();
            sprint.ChangeStatus(SprintStatus.RELEASED);
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Start());
        }

        [TestMethod]
        public void CancelledState_Finish_ThrowsException()
        {
            var sprint = NewSprint();
            sprint.Start();
            sprint.Finish();
            sprint.ChangeStatus(SprintStatus.CANCELLED);
            Assert.ThrowsException<InvalidOperationException>(() => sprint.Finish());
        }
    }
}
