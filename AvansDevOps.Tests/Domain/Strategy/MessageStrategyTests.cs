using System;
using AvansDevOps.Domain.Strategy;
using AvansDevOps.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Strategy
{
    [TestClass]
    public class MessageStrategyTests
    {
        [TestMethod]
        public void OwnerStrategy_GeeftGoudKleur()
        {
            Developer dev = new Developer("Owner", Role.PRODUCTOWNER);
            OwnerStrategy strategy = new OwnerStrategy("Owner message", dev);
            
            Assert.AreEqual("#FFD700", strategy.GetColor());
            Assert.AreEqual("Owner message", strategy.Content);
        }

        [TestMethod]
        public void GuestStrategy_GeeftWitKleur()
        {
            Developer dev = new Developer("Guest", Role.TESTER);
            GuestStrategy strategy = new GuestStrategy("Guest message", dev);
            
            Assert.AreEqual("#FFFFFF", strategy.GetColor());
            Assert.AreEqual("Guest message", strategy.Content);
        }
    }
}
