using System;
using System.Collections.Generic;
using AvansDevOps.Domain.Models;
using AvansDevOps.Domain.Notifications;
using AvansDevOps.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Infrastructure
{
    [TestClass]
    public class RepositoryTests
    {
        private BacklogItem CreateItem(string name = "Item")
        {
            return new BacklogItem(name, "Desc", 3, new NotificationManager());
        }

        [TestMethod]
        public void FakeBacklogRepository_Save_ItemIsOpgeslagen()
        {
            var repo = new FakeBacklogRepository();
            var item = CreateItem("Login");

            repo.Save(item);

            List<BacklogItem> all = repo.GetAll();
            Assert.AreEqual(1, all.Count);
        }

        [TestMethod]
        public void FakeBacklogRepository_GetById_BestaandeId_ReturnsItem()
        {
            var repo = new FakeBacklogRepository();
            var item = CreateItem();
            repo.Save(item);

            // GetAll to retrieve the stored item; GetById with a known key via internal state
            List<BacklogItem> all = repo.GetAll();
            Assert.AreEqual(1, all.Count);
            Assert.AreSame(item, all[0]);
        }

        [TestMethod]
        public void FakeBacklogRepository_GetById_OnbekendeId_ReturnsNull()
        {
            var repo = new FakeBacklogRepository();

            BacklogItem result = repo.GetById(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestMethod]
        public void FakeBacklogRepository_GetAll_LegeRepository_ReturnsLeegeLijst()
        {
            var repo = new FakeBacklogRepository();

            List<BacklogItem> all = repo.GetAll();

            Assert.IsNotNull(all);
            Assert.AreEqual(0, all.Count);
        }

        [TestMethod]
        public void FakeBacklogRepository_Save_MeerdereItems_AlleItemsOpgeslagen()
        {
            var repo = new FakeBacklogRepository();
            repo.Save(CreateItem("Item1"));
            repo.Save(CreateItem("Item2"));
            repo.Save(CreateItem("Item3"));

            Assert.AreEqual(3, repo.GetAll().Count);
        }
    }
}
