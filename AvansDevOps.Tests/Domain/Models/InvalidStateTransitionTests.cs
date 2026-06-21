using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Domain.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Models
{
    [TestClass]
    public class InvalidStateTransitionTests
    {
        private BacklogItem CreateItem()
        {
            return new BacklogItem("Item", "Desc", 3, new NotificationManager());
        }

        private BacklogItem ItemInDoing()
        {
            var item = CreateItem();
            item.Doing();
            return item;
        }

        private BacklogItem ItemInReadyForTesting()
        {
            var item = ItemInDoing();
            item.ReadyForTesting();
            return item;
        }

        private BacklogItem ItemInTesting()
        {
            var item = ItemInReadyForTesting();
            item.Testing();
            return item;
        }

        private BacklogItem ItemInTested()
        {
            var item = ItemInTesting();
            item.Tested();
            return item;
        }

        // --- BacklogItem properties ---

        [TestMethod]
        public void BacklogItem_StateProperty_ReturnsCurrentState()
        {
            var item = CreateItem();
            IBacklogItemState state = item.State;
            Assert.IsNotNull(state);
            Assert.AreEqual("ToDo", state.GetName());
        }

        [TestMethod]
        public void BacklogItem_ActivitiesProperty_ReturnsReadOnlyList()
        {
            var item = CreateItem();
            IReadOnlyList<Activity> activities = item.Activities;
            Assert.IsNotNull(activities);
            Assert.AreEqual(0, activities.Count);
        }

        // --- DoingState ongeldige transities ---

        [TestMethod]
        public void DoingState_Testing_ThrowsInvalidOperationException()
        {
            var item = ItemInDoing();
            Assert.ThrowsException<InvalidOperationException>(() => item.Testing());
        }

        [TestMethod]
        public void DoingState_Tested_ThrowsInvalidOperationException()
        {
            var item = ItemInDoing();
            Assert.ThrowsException<InvalidOperationException>(() => item.Tested());
        }

        [TestMethod]
        public void DoingState_Done_ThrowsInvalidOperationException()
        {
            var item = ItemInDoing();
            Assert.ThrowsException<InvalidOperationException>(() => item.Done());
        }

        // --- ReadyForTestingState ongeldige transities ---

        [TestMethod]
        public void ReadyForTestingState_Doing_ThrowsInvalidOperationException()
        {
            var item = ItemInReadyForTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.Doing());
        }

        [TestMethod]
        public void ReadyForTestingState_Tested_ThrowsInvalidOperationException()
        {
            var item = ItemInReadyForTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.Tested());
        }

        [TestMethod]
        public void ReadyForTestingState_Done_ThrowsInvalidOperationException()
        {
            var item = ItemInReadyForTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.Done());
        }

        // --- TestingState ongeldige transities ---

        [TestMethod]
        public void TestingState_ToDo_ThrowsInvalidOperationException()
        {
            var item = ItemInTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.ToDo());
        }

        [TestMethod]
        public void TestingState_Doing_ThrowsInvalidOperationException()
        {
            var item = ItemInTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.Doing());
        }

        [TestMethod]
        public void TestingState_Testing_ThrowsInvalidOperationException()
        {
            var item = ItemInTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.Testing());
        }

        [TestMethod]
        public void TestingState_Done_ThrowsInvalidOperationException()
        {
            var item = ItemInTesting();
            Assert.ThrowsException<InvalidOperationException>(() => item.Done());
        }

        // --- TestedState ongeldige transities ---

        [TestMethod]
        public void TestedState_Doing_ThrowsInvalidOperationException()
        {
            var item = ItemInTested();
            Assert.ThrowsException<InvalidOperationException>(() => item.Doing());
        }

        [TestMethod]
        public void TestedState_Testing_ThrowsInvalidOperationException()
        {
            var item = ItemInTested();
            Assert.ThrowsException<InvalidOperationException>(() => item.Testing());
        }

        [TestMethod]
        public void TestedState_Tested_ThrowsInvalidOperationException()
        {
            var item = ItemInTested();
            Assert.ThrowsException<InvalidOperationException>(() => item.Tested());
        }
    }
}
